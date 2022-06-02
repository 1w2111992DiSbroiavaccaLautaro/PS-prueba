using System;
using System.Collections.Generic;

#nullable disable

namespace ApiPracticaFinal.Models
{
    public partial class Publicacionesxproyecto
    {
        public int IdPublicacion { get; set; }
        public int? IdProyecto { get; set; }
        public string Publicacion { get; set; }
        public string Año { get; set; }
        public string Medio { get; set; }
        public string Codigobcs { get; set; }
        public char? Trial131 { get; set; }
        public char? Trial278 { get; set; }
        public char? Trial500 { get; set; }

        public virtual Proyecto IdProyectoNavigation { get; set; }
    }
}
