using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.PresupuestoDTOs;
using ApiPracticaFinal.Models.DTO.ProyectoDTOs;
using ApiPracticaFinal.Repository.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.Proyectos
{
    public interface IProyectoRepository
    {
        Task<List<ProyectoDTO>> GetProyectos();
        Task<List<ProyectoDTO>> GetProyectoId(int id);
        Task<List<ProyectoTablaDTO>> GetProyectoTabla(ProyectoQueryFilters filters);
        Task<bool> Create(ProyectoInsert proyecto);
        Task<bool> Update(ProyectoUpdate proyecto);
        Task<bool> Delete(int id);
        List<ProyectoTablaDTO> GetListaProyectos(ProyectoQueryFilters filters);
        bool Impresion(ProyectoQueryFilters filters);
        Task<List<PresupuestoxProyecto>> ListaPresupuestoxProyecto(PresupuestoFilters filters);
    }
}
