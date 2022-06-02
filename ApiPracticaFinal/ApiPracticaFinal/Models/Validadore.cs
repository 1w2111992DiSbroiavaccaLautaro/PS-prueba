using System;
using System.Collections.Generic;

#nullable disable

namespace ApiPracticaFinal.Models
{
    public partial class Validadore
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Titulo { get; set; }
        public string Sector { get; set; }
        public char? Trial147 { get; set; }
        public char? Trial301 { get; set; }
        public char? Trial522 { get; set; }
        public bool? Activo { get; set; }
    }
}
