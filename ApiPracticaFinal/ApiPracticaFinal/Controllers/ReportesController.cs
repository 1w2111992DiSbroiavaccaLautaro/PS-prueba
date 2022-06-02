using ApiPracticaFinal.Models.DTO;
using ApiPracticaFinal.Models.DTO.Reportes;
using ApiPracticaFinal.Repository.Proyectos;
using ApiPracticaFinal.Repository.QueryFilters;
using ApiPracticaFinal.Repository.ReportesRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiPracticaFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Prog3")]
    [Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly IProyectoRepository proyectoRepository;
        private readonly IReportesRepository reporte1Repository;
        public ReportesController(IReportesRepository reporte1Repository , IProyectoRepository proyectoRepository)
        {
            this.reporte1Repository = reporte1Repository;
            this.proyectoRepository = proyectoRepository;
        }
        // GET: api/<ReportesController>
        [HttpGet("Reporte1")]
        public async Task<Reporte1DTO> Reporte1([FromQuery] Reporte1Filters filters)
        {
            return await reporte1Repository.Reporte1(filters);
        }

        [HttpGet("Reporte2")]
        public async Task<Reporte2DTO> Reporte2([FromQuery] Reporte2Filter filters)
        {
            return await reporte1Repository.Reporte2(filters);
        }

        [HttpGet("Reporte3")]
        public async Task<Reporte3DTO> Reporte3([FromQuery] Reporte3Filters filters)
        {
            return await reporte1Repository.Reporte3(filters);
        }

        [HttpGet("Cotizacion")]
        public async Task<decimal> GetItems()
        {
            return await reporte1Repository.GetCotizacion();
        }

        [HttpGet("ProyectosxDepto")]
        public async Task<ProyectosxDepto> GetProyectosxDepto([FromQuery] ValidadorFilter filter)
        {
            return await reporte1Repository.ProyectosxDepto(filter);
        }
        
    }
}
