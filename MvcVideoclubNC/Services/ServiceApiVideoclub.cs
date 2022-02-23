using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using MvcVideoclubNC.Models;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;


namespace MvcVideoclubNC.Services
{
    public class ServiceApiVideoclub
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceApiVideoclub(string url)
        {
            this.UrlApi = url;
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
        }


        //Método sin seguridad
        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                string url = this.UrlApi + request;

                HttpResponseMessage response =
                    await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    T data =
                        await response.Content.ReadAsAsync<T>();
                    return data;
                } else
                {
                    return default(T);
                }
            }
        }

        //Método con seguridad
        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                string url = this.UrlApi + request;
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);

                HttpResponseMessage response =
                    await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    T data =
                        await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //Método para obtener el token
        public async Task<string> GetTokenAsync(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/auth/login";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                LoginModel model = new LoginModel();
                model.Username = username;
                model.Password = password;

                string json = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");

                client.BaseAddress = new Uri(this.UrlApi);

                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data =
                        await response.Content.ReadAsStringAsync();
                    JObject jobject = JObject.Parse(data);
                    string token =
                        jobject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Genero>> GetGenerosAsync()
        {
            string request = "/api/peliculas/generos";
            List<Genero> generos =
                await this.CallApiAsync<List<Genero>>(request);
            return generos;
        }

        public async Task<List<Pelicula>> GetPeliculasGeneroAsync(int idgenero)
        {
            string request = "/api/peliculas/peliculasgenero/" + idgenero;
            List<Pelicula> peliculas =
                await this.CallApiAsync<List<Pelicula>>(request);
            return peliculas;
        }

        public async Task<List<Pelicula>> GetPeliculasAsync()
        {
            string request = "/api/peliculas";
            List<Pelicula> peliculas =
                await this.CallApiAsync<List<Pelicula>>(request);
            return peliculas;
        }

        public async Task<Pelicula> FindPeliculaAsync(int idpelicula)
        {
            string request = "/api/peliculas/" + idpelicula;
            Pelicula pelicula =
                await this.CallApiAsync<Pelicula>(request);
            return pelicula;
        }





        //Métodos con el carrito
        public async Task<List<Pelicula>> GetCarritoPeliculasAsync(List<int> carrito)
        {
            string request = "/api/peliculas";
            List<Pelicula> peliculas =
                await this.CallApiAsync<List<Pelicula>>(request);
            var consulta = from datos in peliculas
                           where carrito.Contains(datos.IdPelicula)
                           select datos;
            return consulta.ToList();
        }

        //Métodos con seguridad


        public async Task<Cliente> GetPerfilClienteAsync(string token)
        {

            string request = "/api/peliculas/perfilcliente";
            Cliente cliente =
                await this.CallApiAsync<Cliente>(request, token);
            return cliente;

        }

        public async Task<List<ClientesPeliculasPedido>> GetPeliculasPedidosClienteAsync(string token)
        {
            string request = "/api/peliculas/pedidoscliente";
            List<ClientesPeliculasPedido> pedidosCliente =
                await this.CallApiAsync<List<ClientesPeliculasPedido>>(request,token);
            return pedidosCliente;
        }

        public async Task AddPedidoCliente(
            int idcliente, int idpelicula, int cantidad,
            DateTime fecha, int precio, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/peliculas/addpedido";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Pedido pedido = new Pedido();

                pedido.IdCliente = idcliente;
                pedido.IdPelicula = idpelicula;
                pedido.Cantidad = cantidad;
                pedido.Fecha = fecha;
                pedido.Precio = precio;

                string json = JsonConvert.SerializeObject(pedido);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }
    }
}
