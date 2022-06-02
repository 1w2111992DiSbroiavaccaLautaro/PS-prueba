using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Models.DTO.PersonalesDTO
{
    public class PersonalInsert
    {
        public string Nombre { get; set; }
        public bool? Activo { get; set; }
    }
}
