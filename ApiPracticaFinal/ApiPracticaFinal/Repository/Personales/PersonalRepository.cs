using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.PersonalesDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.Personales
{
    public class PersonalRepository : IPersonalRepository
    {

        private readonly dd4snj9pkf64vpContext context;

        public PersonalRepository(dd4snj9pkf64vpContext context)
        {
            this.context = context;
        }

        public async Task<Personal> Create(PersonalInsert insert)
        {
            var personal = new Personal
            {
                Nombre = insert.Nombre,
                Activo = true
            };

            if (personal != null)
            {
                context.Personals.Add(personal);
                await context.SaveChangesAsync();
                return personal;
            }
            else
            {
                throw new Exception("No se pudo insertar el personal;");
            }
        }

        public async Task<bool> Delete(int id)
        {
            var p = await context.Personals.FindAsync(id);
            if (p != null)
            {
                p.Activo = false;
                context.Personals.Update(p);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new Exception("No se pudo encontrar el personal");
            }
        }

        public async Task<List<Personal>> GetPersonalAsync()
        {
            return await context.Personals.Where(x => x.Activo == true).OrderBy(x => x.Nombre).ToListAsync();
        }

        public async Task<List<Personal>> GetPersonalId(int id)
        {
            return await context.Personals.Where(x => x.Id == id && x.Activo == true).ToListAsync();
        }

        public async Task<List<PersonalxProyectoDTO>> GetPersonalxProyecto(int idPersonal, bool? FichaLista)
        {
            var proyectos = await context.Proyectos.Where(x => x.Activo == true).ToListAsync();
            var equipoxproyectoBD = await context.Equipoxproyectos.Where(x => x.IdPersonal == idPersonal).ToListAsync();
            var personalBD = await context.Personals.Where(x => x.Activo == true).ToListAsync();

            var listaPersonalxProyecto = new List<PersonalxProyectoDTO>();

            int index = 0;

            foreach (var i in proyectos)
            {
                var proyectosxpersonal = equipoxproyectoBD.Where(x => x.IdProyecto == i.Id).ToList();
                
                foreach (var j in proyectosxpersonal)
                {
                    var persona = personalBD.FirstOrDefault(x => x.Id == idPersonal);

                    index++;

                    var coordinar = j.Coordinador == true ? "Coordinador" : "No coordinador";

                    var fichaLista = i.FichaLista == true ? "Ficha Lista" : "Ficha no Lista";

                    var personalDTO = new PersonalxProyectoDTO
                    {
                        Nombre = persona.Nombre,
                        EsCoordinador = j.Coordinador,
                        Titulo = i.Titulo,
                        EsFichaLista = i.FichaLista,
                        Coordinador = coordinar,
                        FichaLista = fichaLista,
                        AnioInicio = i.AnioInicio,
                        AnioFin = i.AnioFinalizacion,
                    };
                    personalDTO.CantidadProyectos = index;
                    listaPersonalxProyecto.Add(personalDTO);
                }
            }

            if(FichaLista != null)
            {
                listaPersonalxProyecto = listaPersonalxProyecto.Where(x => x.EsFichaLista == FichaLista).ToList();
            }

            return listaPersonalxProyecto;
        }

        public async Task<Personal> Update(PersonalUpdate update)
        {
            var p = context.Personals.FirstOrDefault(x => x.Id == update.Id);
            if (p != null)
            {
                p.Nombre = update.Nombre ?? p.Nombre;
                p.Activo = true;

                context.Personals.Update(p);
                await context.SaveChangesAsync();
                return p;
            }
            else
            {
                throw new Exception("No se pudo actualizar");
            }
        }
    }
}
