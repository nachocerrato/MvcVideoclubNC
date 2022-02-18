using Microsoft.AspNetCore.Mvc;
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


            return View(pelicula);
        }


    }
}
