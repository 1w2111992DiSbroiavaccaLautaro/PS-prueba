using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.QueryFilters
{
    public class Reporte2Filter
    {
        public int? AnioDesde { get; set; }
        public int? AnioHasta { get; set; }
        public bool? Estado { get; set; }
    }
}
