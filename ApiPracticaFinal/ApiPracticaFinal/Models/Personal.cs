using System;
using System.Collections.Generic;

#nullable disable

namespace ApiPracticaFinal.Models
{
    public partial class Personal
    {
        public Personal()
        {
            Equipoxproyectos = new HashSet<Equipoxproyecto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Titulo { get; set; }
        public string Sector { get; set; }
        public bool? Activo { get; set; }
        public char? Trial467 { get; set; }

        public virtual ICollection<Equipoxproyecto> Equipoxproyectos { get; set; }
    }
}
