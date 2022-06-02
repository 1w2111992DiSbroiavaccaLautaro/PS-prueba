using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Models.DTO.UsuariosDTO
{
    public class UsuarioUpdatePass
    {
        public string Email { get; set; }
        public string PasswordVieja { get; set; }
        public string PasswordNueva { get; set; }
    }
}
