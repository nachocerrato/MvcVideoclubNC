using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Models
{

    public class ClientesPeliculasPedido
    {

        public int IdPedido { get; set; }

        public int IdCliente { get; set; }

        public String Nombre { get; set; }

        public String Titulo { get; set; }

        public int Cantidad { get; set; }

        public int Precio { get; set; }
    }
}
