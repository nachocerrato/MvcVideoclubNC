using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Models
{

    public class Administrador
    {

        public int IdAdmin { get; set; }

        public String Nombre { get; set; }

        public String Mail { get; set; }
    }
}
