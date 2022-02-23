using Microsoft.AspNetCore.Mvc;
using MvcVideoclubNC.Extensions;
using MvcVideoclubNC.Filters;
using MvcVideoclubNC.Models;
using MvcVideoclubNC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Controllers
{
    public class ClientesController : Controller
    {
        private ServiceApiVideoclub service;

        public ClientesController(ServiceApiVideoclub service)
        {
            this.service = service;
        }

        [AuthorizeClientes]
        public async Task<IActionResult> Perfil()
        {
            string token =
                HttpContext.User.FindFirst("TOKEN").Value;
            Cliente cliente = 
                await this.service.GetPerfilClienteAsync(token);
            return View(cliente);
        }

        [AuthorizeClientes]
        public async Task<IActionResult> Pedidos()
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            List<ClientesPeliculasPedido> pedidosCliente =
                await this.service.GetPeliculasPedidosClienteAsync(token);
            return View(pedidosCliente);
        }

        [AuthorizeClientes]
        public async Task<IActionResult> FinalizarPedidoCliente()
        {
            List<int> carrito =
                HttpContext.Session.GetObject<List<int>>("CARRITO");
            List<Pelicula> peliculas =
                await this.service.GetCarritoPeliculasAsync(carrito);
            string datacliente =
                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string token =
                HttpContext.User.FindFirst("TOKEN").Value;
            int idcliente = int.Parse(datacliente);
            
            foreach(Pelicula peli in peliculas)
            {
                await this.service.AddPedidoCliente(
                    idcliente, peli.IdPelicula,1,DateTime.Now,peli.Precio,token);
            }

            HttpContext.Session.Remove("CARRITO");

            return RedirectToAction("Pedidos", "Clientes");

        }
    }
}
