using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Models.DTO.PersonalesDTO
{
    public class PersonalxProyectoDTO
    {
        public string Nombre { get; set; }
        public string Titulo { get; set; }
        public bool? EsCoordinador { get; set; }
        public string Coordinador { get; set; }
        public string FichaLista { get; set; }
        public bool? EsFichaLista { get; set; }
        public int? AnioInicio { get; set; }
        public int? AnioFin { get; set; }
        public int CantidadProyectos { get; set; }
    }
}
