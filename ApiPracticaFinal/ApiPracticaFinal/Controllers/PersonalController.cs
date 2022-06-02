using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.PersonalesDTO;
using ApiPracticaFinal.Repository.Personales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiPracticaFinal.Controllers.Personales
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalController : ControllerBase
    {
        private readonly IPersonalRepository personalRepository;
        public PersonalController(IPersonalRepository personalRepository)
        {
            this.personalRepository = personalRepository;
        }
        // GET: api/<PersonalController>
        [HttpGet]
        public async Task<List<Personal>> GetPersonal()
        {
            return await personalRepository.GetPersonalAsync();
        }

        // GET api/<PersonalController>/5
        [HttpGet("{id}")]
        public async Task<List<Personal>> GetId(int id)
        {
            return await personalRepository.GetPersonalId(id);
        }

        [HttpGet("personalxproyecto")]
        public async Task<List<PersonalxProyectoDTO>> GetPersoalxProyecto([FromQuery] int id, bool? fichaLista)
        {
            return await personalRepository.GetPersonalxProyecto(id, fichaLista);
        }

        // POST api/<PersonalController>
        [HttpPost]
        public async Task<Personal> Post(PersonalInsert insert)
        {
            return await personalRepository.Create(insert);
        }

        // PUT api/<PersonalController>/5
        [HttpPut]
        public async Task<Personal> Put(PersonalUpdate update)
        {
            return await personalRepository.Update(update);
        }

        // DELETE api/<PersonalController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await personalRepository.Delete(id);
        }
    }
}
