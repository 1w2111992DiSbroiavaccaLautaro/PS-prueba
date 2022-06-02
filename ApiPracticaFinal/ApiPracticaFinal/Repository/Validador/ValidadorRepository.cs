using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.Validadores;
using ApiPracticaFinal.Repository.QueryFilters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.Validador
{
    public class ValidadorRepository : IValidadorRepository
    {
        private readonly dd4snj9pkf64vpContext context;

        public ValidadorRepository(dd4snj9pkf64vpContext context)
        {
            this.context = context;
        }
        public async Task<Validadore> Create(ValidadorInsert validador)
        {
            var v = new Validadore
            {
                Nombre = validador.Nombre,
                Titulo = validador.Titulo,
                Activo = true
            };

            if (v != null)
            {
                await context.Validadores.AddAsync(v);
                await context.SaveChangesAsync();
                return v;
            }
            else
            {
                throw new Exception("No se pudo insertar");
            }
        }

        public async Task<bool> Delete(int id)
        {
            var v = await context.Validadores.FirstOrDefaultAsync(x => x.Id == id && x.Activo == true);
            if (v != null)
            {
                v.Activo = false;
                context.Validadores.Update(v);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new Exception("No se pudo eliminar");
            }
        }

        public async Task<List<Validadore>> GetValidadores()
        {
            return await context.Validadores.Where(x => x.Activo == true).OrderBy(x => x.Nombre).ToListAsync();
        }

        public async Task<Validadore> GetValidadorId(int id)
        {
            return await context.Validadores.FirstOrDefaultAsync(x => x.Id == id && x.Activo == true);
        }

        public async Task<Validadore> Update(ValidadorUpdate validador)
        {
            var v = await context.Validadores.FirstOrDefaultAsync(x => x.Id == validador.Id && x.Activo == true);

            if (v != null)
            {
                v.Nombre = validador.Nombre ?? v.Nombre;
                v.Titulo = validador.Titulo ?? v.Titulo;

                context.Validadores.Update(v);
                await context.SaveChangesAsync();
                return v;
            }
            else
            {
                throw new Exception("No se pudo actualizar");
            }
        }

        public async Task<ValidadoresxProyectoDTO> ValidadorxProyecto(ValidadorFilter filters)
        {
            var proyectos = new List<Proyecto>();

            if (filters.AnioDesde != 0 && filters.AnioFin != 0)
            {
                proyectos = await context.Proyectos.Where
                    (x => x.Activo == true && x.FichaLista == true && x.AnioInicio >= filters.AnioDesde 
                    && x.AnioFinalizacion <= filters.AnioFin).ToListAsync();
            }
            else
            {
                proyectos = await context.Proyectos.Where(x => x.Activo == true && x.FichaLista == true).ToListAsync();
            }

            var cantidad0 = proyectos.Where(x => x.Certificadopor == 0).Count();
            var cantidad1 = proyectos.Where(x => x.Certificadopor == 1).Count();
            var cantidad2 = proyectos.Where(x => x.Certificadopor == 2).Count();
            var cantidad3 = proyectos.Where(x => x.Certificadopor == 3).Count();
            var cantidad4 = proyectos.Where(x => x.Certificadopor == 4).Count();
            var cantidad5 = proyectos.Where(x => x.Certificadopor == 5).Count();

            var validadorDto = new ValidadoresxProyectoDTO
            {
                Cantidad0 = cantidad0,
                Cantidad1 = cantidad1,
                Cantidad2 = cantidad2,
                Cantidad3 = cantidad3,
                Cantidad4 = cantidad4,
                Cantidad5 = cantidad5
            };

            return validadorDto;
        }

    }
}
