using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.PersonalesDTO;

namespace ApiPracticaFinal.Repository.Personales
{
    public interface IPersonalRepository
    {
        Task<List<PersonalxProyectoDTO>> GetPersonalxProyecto(int idPersonal, bool? fichaLista);
        Task<List<Personal>> GetPersonalAsync();
        Task<List<Personal>> GetPersonalId(int id);
        Task<Personal> Create(PersonalInsert insert);
        Task<Personal> Update(PersonalUpdate update);
        Task<bool> Delete(int id);
    }
}
