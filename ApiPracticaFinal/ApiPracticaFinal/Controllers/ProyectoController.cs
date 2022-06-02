using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.PresupuestoDTOs;
using ApiPracticaFinal.Models.DTO.ProyectoDTOs;
using ApiPracticaFinal.Repository.Proyectos;
using ApiPracticaFinal.Repository.QueryFilters;
using ApiPracticaFinal.Repository.SendEmailGmail;
using ApiPracticaFinal.Repository.SendGrid;
using ApiPracticaFinal.Repository.Usuarios;
using Aspose.Words;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiPracticaFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Prog3")]
    //[Authorize]
    public class ProyectoController : ControllerBase
    {
        private readonly IProyectoRepository proyectoRepository;
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;

        public ProyectoController(IProyectoRepository proyectoRepository, IUsuarioRepository usuarioRepository, IEmailSender emailSender, IConfiguration configuration)
        {
            this.proyectoRepository = proyectoRepository;
            this.usuarioRepository = usuarioRepository;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }
        // GET: api/<ProyectoController>
        [HttpGet]
        public async Task<List<ProyectoDTO>> Get()
        {
            return await proyectoRepository.GetProyectos();
        }

        [HttpGet("husoHorario")]
        public DateTime GetHusoHorario()
        {
            try
            {
                TimeZoneInfo America_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
                DateTime dateTime_america = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, America_Standard_Time);
                return dateTime_america;

            }
            catch (Exception e)
            {
                throw new Exception("e " + e.Message);
            }
        }

        [HttpGet("tabla")]
        public async Task<List<ProyectoTablaDTO>> GetTabla([FromQuery] ProyectoQueryFilters filters)
        {
            return await proyectoRepository.GetProyectoTabla(filters);
        }

        [HttpGet("impresion")]
        public bool Impresion([FromQuery] ProyectoQueryFilters filters)
        {
            return proyectoRepository.Impresion(filters);
        }

        [HttpGet("lista")]
        public List<ProyectoTablaDTO> GetListaProyectos([FromQuery] ProyectoQueryFilters filters)
        {
            return proyectoRepository.GetListaProyectos(filters);
        }

        // GET api/<ProyectoController>/5
        [HttpGet("{id}")]
        public async Task<List<ProyectoDTO>> GetId(int id)
        {
            return await proyectoRepository.GetProyectoId(id);
        }

        [HttpGet("presupuestoxProyecto")]
        public async Task<List<PresupuestoxProyecto>> GetPresupuestoxProyectos([FromQuery] PresupuestoFilters filters)
        {
            return await proyectoRepository.ListaPresupuestoxProyecto(filters);
        }

        // POST api/<ProyectoController>
        [HttpPost]
        public async Task<bool> Post(ProyectoInsert proyecto)
        {
            var hora = DateTime.Now.ToString("HH");
            var min = DateTime.Now.ToString("mm");

            var hora2 = Convert.ToInt32(hora);
            var hora3 = hora2 - 3;

            var insertada = await proyectoRepository.Create(proyecto);
            var usuarios = usuarioRepository.GetUsuariosMail();

            if (insertada)
            {
                //await emailSender.SendEmailAsync("Nueva Ficha registrada", "Una nueva ficha fue Registrada a las " + dateTime_america.ToString("dd/MM/yyyy") +
                //    " a las " + dateTime_america.ToString("HH:mm tt"), usuarios);
                await emailSender.SendEmailAsync("Nueva Ficha registrada", "Una nueva ficha fue Registrada a las " + DateTime.Now.ToString("dd/MM/yyyy") +
                    " a las " + hora3 + ":" + min, usuarios);
            }

            return insertada;
        }

        // PUT api/<ProyectoController>/5
        [HttpPut]
        public async Task<bool> Put(ProyectoUpdate proyecto)
        {
            var hora = DateTime.Now.ToString("HH");
            var min = DateTime.Now.ToString("mm");

            var hora2 = Convert.ToInt32(hora);
            var hora3 = hora2 - 3;

            var actualizada = await proyectoRepository.Update(proyecto);
            var usuarios = usuarioRepository.GetUsuariosMail();

            if (actualizada)
            {
                await emailSender.SendEmailAsync("Ficha actualizada", "La Ficha " + proyecto.IdProyecto + " fue actualizada a las " + DateTime.Now.ToString("dd/MM/yyyy") +
                    " a las " + hora3 + ":" + min, usuarios);
            }
            //var validadores = usuarioRepository.ListaValidadoresMail();
            //string url = $"{configuration["AppUrl"]}/api/proyecto/confirmarFicha/" + proyecto.IdProyecto;

            return actualizada;
        }

        [HttpGet("confirmarFicha/{IdProyecto}")]
        public async Task<List<ProyectoDTO>> ConfirmarFicha(int IdProyecto)
        {
            var result = await proyectoRepository.GetProyectoId(IdProyecto);

            if(result != null)
            {
                Redirect($"{configuration["AppUrl"]}/confirmarFicha.html");
            }
            return result;
        }

        // DELETE api/<ProyectoController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            var hora = DateTime.Now.ToString("HH");
            var min = DateTime.Now.ToString("mm");

            var hora2 = Convert.ToInt32(hora);
            var hora3 = hora2 - 3;

            var eliminado = await proyectoRepository.Delete(id);
            var usuarios = usuarioRepository.GetUsuariosMail();

            if (eliminado)
            {
                await emailSender.SendEmailAsync("Ficha eliminada", "La Ficha " + id + " fue eliminada el dia " + DateTime.Now.ToString("dd/MM/yyyy") + 
                    " a las " + hora3 + ":" + min, usuarios);
            }

            return eliminado;
        }
    }
}
