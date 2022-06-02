using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.Validadores;
using ApiPracticaFinal.Repository.QueryFilters;
using ApiPracticaFinal.Repository.Validador;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize]
    public class ValidadorController : ControllerBase
    {
        private readonly IValidadorRepository validadorRepository;
        public ValidadorController(IValidadorRepository validadorRepository)
        {
            this.validadorRepository = validadorRepository;
        }
        // GET: api/<ValidadorController>
        [HttpGet]
        public async Task<List<Validadore>> Get()
        {
            return await validadorRepository.GetValidadores();
        }

        [HttpGet("validadorxProyecto")]
        public async Task<ValidadoresxProyectoDTO> GetValidadoresxProyecto([FromQuery] ValidadorFilter filter)
        {
            return await validadorRepository.ValidadorxProyecto(filter);
        }

        // GET api/<ValidadorController>/5
        [HttpGet("{id}")]
        public async Task<Validadore> GetId(int id)
        {
            return await validadorRepository.GetValidadorId(id);
        }

        // POST api/<ValidadorController>
        [HttpPost]
        public async Task<Validadore> Post(ValidadorInsert v)
        {
            return await validadorRepository.Create(v);
        }

        // PUT api/<ValidadorController>/5
        [HttpPut]
        public async Task<Validadore> Put(ValidadorUpdate v)
        {
            return await validadorRepository.Update(v);
        }

        // DELETE api/<ValidadorController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await validadorRepository.Delete(id);
        }
    }
}
