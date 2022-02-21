using Microsoft.AspNetCore.Mvc;
using MvcVideoclubNC.Extensions;
using MvcVideoclubNC.Models;
using MvcVideoclubNC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Controllers
{
    public class PeliculasController : Controller
    {

        private ServiceApiVideoclub service;
        private ServiceStorageBlobs serviceBlob;

        public PeliculasController(ServiceApiVideoclub service, ServiceStorageBlobs serviceBlob)
        {
            this.service = service;
            this.serviceBlob = serviceBlob;
        }
        public async Task<IActionResult> PeliculasGenero(int idgenero)
        {
            List<Pelicula> peliculas =
                await this.service.GetPeliculasGeneroAsync(idgenero);

            string containerName = "blobsvideoclubnc";

            List<Blob> blobs =
                await this.serviceBlob.GetBlobsAsync(containerName);
            ViewData["BLOBS"] = blobs;


            return View(peliculas);
        }

        public async Task<IActionResult> DetailsPelicula(int idpelicula)
        {
            Pelicula pelicula =
                await this.service.FindPeliculaAsync(idpelicula);
            string imagen = pelicula.Foto;


            string containerName = "blobsvideoclubnc";
            string blobUrl = this.serviceBlob.GetBlobUrl(containerName, imagen);
            ViewData["BLOB"] = blobUrl;
            ViewData["YOUTUBE"] = "https://www.youtube.com/embed/";

            return View(pelicula);
        }

        //Método para utilizar Sessions añadiendo películas al carrito
        public IActionResult AddPeliculaCarrito(int idpelicula)
        {
            List<int> idspeliculas;

            if(HttpContext.Session.GetObject<List<int>>("CARRITO") == null)
            {
                idspeliculas = new List<int>();
            }
            else
            {
                idspeliculas =
                    HttpContext.Session.GetObject<List<int>>("CARRITO");
            }

            idspeliculas.Add(idpelicula);
            HttpContext.Session.SetObject<List<int>>("CARRITO", idspeliculas);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CarritoCompra(int? ideliminar)
        {
            List<int> carrito =
                HttpContext.Session.GetObject<List<int>>("CARRITO");
            if(carrito == null)
            {
                ViewData["MENSAJE"] = "No hay películas en tu carrito.";
                return View();
            }
            else
            {
                if (ideliminar != null)
                {
                    carrito.Remove(ideliminar.Value);
                    if(carrito.Count == 0)
                    {
                        HttpContext.Session.Remove("CARRITO");
                        return View();
                    }
                    else
                    {
                        HttpContext.Session.SetObject<List<int>>("CARRITO", carrito);
                    }
                }
                List<Pelicula> peliculas =
                    await this.service.GetCarritoPeliculasAsync(carrito);
                return View(peliculas);
            }
        }
    }
}
