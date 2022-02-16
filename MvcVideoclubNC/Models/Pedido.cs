using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Models
{

    public class Pedido
    {


        public int IdCliente { get; set; }

        public int IdPelicula { get; set; }

        public int Cantidad { get; set; }

        public DateTime Fecha { get; set; }

        public int Precio { get; set; }
    }
}
