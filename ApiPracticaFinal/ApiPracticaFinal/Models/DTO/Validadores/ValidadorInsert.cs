using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Models.DTO.Validadores
{
    public class ValidadorInsert
    {
        public string Nombre { get; set; }
        public string Titulo { get; set; }
        public bool? Activo { get; set; }
    }
}
