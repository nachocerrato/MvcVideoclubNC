using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Models
{

    public class Genero
    {

        public int IdGenero { get; set; }

        public String NombreGenero { get; set; }
    }
}
