using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.Validadores;
using ApiPracticaFinal.Repository.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.Validador
{
    public interface IValidadorRepository
    {
        Task<List<Validadore>> GetValidadores();
        Task<Validadore> GetValidadorId(int id);
        Task<Validadore> Create(ValidadorInsert validador);
        Task<Validadore> Update(ValidadorUpdate validador);
        Task<bool> Delete(int id);
        Task<ValidadoresxProyectoDTO> ValidadorxProyecto(ValidadorFilter filters);
    }
}
