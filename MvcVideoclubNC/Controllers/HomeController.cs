using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcVideoclubNC.Models;
using MvcVideoclubNC.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ServiceApiVideoclub service;
        private ServiceStorageBlobs serviceBlob;

        public HomeController(ILogger<HomeController> logger, 
            ServiceApiVideoclub service, ServiceStorageBlobs serviceBlob)
        {
            _logger = logger;
            this.service = service;
            this.serviceBlob = serviceBlob;
        }

        public async Task<IActionResult> Index()
        {
            List<Pelicula> peliculas =
                await this.service.GetPeliculasAsync();

            string containerName = "blobsvideoclubnc";

            List<Blob> blobs =
                await this.serviceBlob.GetBlobsAsync(containerName);
            ViewData["BLOBS"] = blobs;

            return View(peliculas);
        }

        //public IActionResult Index()
        //{

        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
