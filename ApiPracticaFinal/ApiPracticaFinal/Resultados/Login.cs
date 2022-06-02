using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Resultados
{
    public class Login
    {
        
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public int Rol { get; set; }

        public Login(int Id, string Email, string Token, int Rol)
        {
            this.Id = Id;
            this.Token = Token;
            this.Email = Email;
            this.Rol = Rol;
        }
    }
}
