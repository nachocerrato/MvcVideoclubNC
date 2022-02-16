using Microsoft.AspNetCore.Mvc;
using MvcVideoclubNC.Models;
using MvcVideoclubNC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Components
{
    public class MenuGenerosViewComponent : ViewComponent
    {
        private ServiceApiVideoclub service;

        public MenuGenerosViewComponent(ServiceApiVideoclub service)
        {
            this.service = service;
        }

        //Todo Component tiene un método para devolverle el dibujo al Layout
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos =
                await this.service.GetGenerosAsync();
            return View(generos);
        }
    }
}
