using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.Areas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.Areas
{
    public interface IAreaRepository
    {
        Task<List<Area>> GetAreas();
        Task<List<AreaxDeptoDTO>> GetAreasxDepto(string depto);
        Task<Area> Create(AreaInsertDTO area);
        Task<Area> Update(AreaUpdateDTO area);
        Task<bool> Delete(int id);
        Task<AreaDTO> GetAreaId(int id);

    }
}
