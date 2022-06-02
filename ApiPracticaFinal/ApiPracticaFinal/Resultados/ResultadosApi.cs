using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Resultados
{
    public class ResultadosApi
    {
        public bool Ok { get; set; }

        public string Error { get; set; }

        public string InfoAdicional { get; set; }

        public object Respuesta { get; set; }
    }
}
