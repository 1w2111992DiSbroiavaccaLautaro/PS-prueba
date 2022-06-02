using ApiPracticaFinal.Models.DTO;
using ApiPracticaFinal.Models.DTO.Reportes;
using ApiPracticaFinal.Repository.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ApiPracticaFinal.Models.DTO.CotizacionDTO;

namespace ApiPracticaFinal.Repository.ReportesRepo
{
    public interface IReportesRepository
    {
        Task<Reporte1DTO> Reporte1(Reporte1Filters filters);
        Task<Reporte2DTO> Reporte2(Reporte2Filter filters);
        Task<Reporte3DTO> Reporte3(Reporte3Filters filters);
        Task<decimal> GetCotizacion();
        Task<ProyectosxDepto> ProyectosxDepto(ValidadorFilter filter);
    }
}
