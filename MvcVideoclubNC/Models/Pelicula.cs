using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Models
{
    public class Pelicula
    {

        public int IdPelicula { get; set; }
        public int IdGenero { get; set; }
        public String Titulo { get; set; }

        public String Argumento { get; set; }

        public String Foto { get; set; }

        public DateTime FechaEstreno { get; set; }

        public String Actores { get; set; }

        public String Director { get; set; }

        public int Duracion { get; set; }

        public int Precio { get; set; }

        public String Youtube { get; set; }
    }
}
