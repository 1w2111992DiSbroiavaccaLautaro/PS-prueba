using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.Areas;
using ApiPracticaFinal.Repository.Areas;
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
    public class AreaController : ControllerBase
    {
        private readonly IAreaRepository areaRepository;

        public AreaController(IAreaRepository areaRepository)
        {
            this.areaRepository = areaRepository;
        }
        // GET: api/<AreaController>
        [HttpGet]
        public async Task<List<Area>> Get()
        {
            return await areaRepository.GetAreas();
        }

        // GET api/<AreaController>/5
        [HttpGet("{id}")]
        public async Task<AreaDTO> GetId(int id)
        {
            return await areaRepository.GetAreaId(id);
        }

        [HttpGet("areaxdepto")]
        public async Task<List<AreaxDeptoDTO>> GetAreaxDepto([FromQuery] string depto)
        {
            return await areaRepository.GetAreasxDepto(depto);
        }

        // POST api/<AreaController>
        [HttpPost]
        public async Task<Area> Post(AreaInsertDTO area)
        {
            return await areaRepository.Create(area);
        }

        // PUT api/<AreaController>/5
        [HttpPut]
        public async Task<Area> Put(AreaUpdateDTO area)
        {
            return await areaRepository.Update(area);
        }

        // DELETE api/<AreaController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await areaRepository.Delete(id);
        }
    }
}
