using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Models.DTO.PersonalesDTO
{
    public class PersonalGetIdDTO
    {
        public int IdPersonal { get; set; }
        public string Nombre { get; set; }
        public bool? Coordinador { get; set; }
    }
}
