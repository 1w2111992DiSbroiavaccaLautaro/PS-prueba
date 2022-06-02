﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Models.DTO.PersonalesDTO
{
    public class PersonalDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool? Activo { get; set; }
        public bool? Coordinador { get; set; }
    }
}
