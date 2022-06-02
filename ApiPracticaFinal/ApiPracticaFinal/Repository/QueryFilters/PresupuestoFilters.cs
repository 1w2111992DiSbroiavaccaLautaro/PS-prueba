using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.QueryFilters
{
    public class PresupuestoFilters
    {
        public int AnioDesde { get; set; }
        public int AnioFin { get; set; }
        public string Depto { get; set; }
    }
}
