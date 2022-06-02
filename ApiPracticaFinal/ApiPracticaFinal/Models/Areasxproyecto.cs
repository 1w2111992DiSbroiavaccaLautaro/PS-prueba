using System;
using System.Collections.Generic;

#nullable disable

namespace ApiPracticaFinal.Models
{
    public partial class Areasxproyecto
    {
        public int Idproyecto { get; set; }
        public int Idarea { get; set; }
        public char? Trial069 { get; set; }
        public char? Trial206 { get; set; }
        public char? Trial451 { get; set; }

        public virtual Area IdareaNavigation { get; set; }
        public virtual Proyecto IdproyectoNavigation { get; set; }
    }
}
