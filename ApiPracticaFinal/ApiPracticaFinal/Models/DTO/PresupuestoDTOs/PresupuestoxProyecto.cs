using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Models.DTO.PresupuestoDTOs
{
    public class PresupuestoxProyecto
    {
        public double Honorario { get; set; }
        public double Viatico { get; set; }
        public double Equipamiento { get; set; }
        public double Gastos { get; set; }
        public string Divisa { get; set; }
        public double? Montototal { get; set; }
        public string Titulo { get; set; }
        public string Depto { get; set; }
        public int? Anioinicio { get; set; }
        public int? AnioFin { get; set; }
    }
}
