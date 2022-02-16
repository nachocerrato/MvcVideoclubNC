using MvcVideoclubNC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

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

        public async Task<List<ClientesPeliculasPedido>> GetPeliculasPedidosClienteAsync(int idcliente)
        {
            string request = "/api/peliculas/pedidoscliente/" + idcliente;
            List<ClientesPeliculasPedido> pedidosCliente =
                await this.CallApiAsync<List<ClientesPeliculasPedido>>(request);
            return pedidosCliente;
        }
    }
}
