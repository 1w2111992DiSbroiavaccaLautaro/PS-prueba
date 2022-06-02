using System;
using System.Collections.Generic;

#nullable disable

namespace ApiPracticaFinal.Models
{
    public partial class Usuario
    {
        public int Idusuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; }
        public int Rol { get; set; }

        public virtual Role RolNavigation { get; set; }
    }
}
