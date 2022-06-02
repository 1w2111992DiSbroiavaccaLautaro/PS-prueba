using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.SendGrid
{
    public class EmailSenderOptions
    {
        public int Port { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }
    }
}
