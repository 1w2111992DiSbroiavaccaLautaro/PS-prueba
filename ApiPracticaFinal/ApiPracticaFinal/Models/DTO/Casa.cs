using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Models.DTO
{
    public class Casa
    {
        public string nombre { get; set; }
        public string compra { get; set; }
        public string venta { get; set; }
        public string mejor_compra { get; set; }
        public string mejor_venta { get; set; }
        public string fecha { get; set; }
        public string recorrido { get; set; }
        public Afluencia afluencia { get; set; }
        public string agencia { get; set; }
        public Observaciones observaciones { get; set; }
    }
}
