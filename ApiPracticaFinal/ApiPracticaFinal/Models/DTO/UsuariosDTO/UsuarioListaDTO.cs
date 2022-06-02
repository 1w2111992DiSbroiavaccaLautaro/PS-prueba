using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Models.DTO.UsuariosDTO
{
    public class UsuarioListaDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public int IdRol { get; set; }
        public string Rol { get; set; }
        
    }
}
