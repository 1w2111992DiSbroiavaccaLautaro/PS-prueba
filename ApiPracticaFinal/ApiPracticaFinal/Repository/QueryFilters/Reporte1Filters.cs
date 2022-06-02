using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.QueryFilters
{
    public class Reporte1Filters
    {
        public string? Departamento { get; set; }
        //hace referncia a si la ficha esta lista o no
        public bool? Estado { get; set; }
    }
}
