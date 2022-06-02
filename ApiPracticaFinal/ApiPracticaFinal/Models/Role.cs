using System;
using System.Collections.Generic;

#nullable disable

namespace ApiPracticaFinal.Models
{
    public partial class Role
    {
        public Role()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Idrol { get; set; }
        public string Rol { get; set; }
        public char? Trial506 { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
