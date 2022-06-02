using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO.Areas;
using ApiPracticaFinal.Models.DTO.PersonalesDTO;
using ApiPracticaFinal.Models.DTO.PresupuestoDTOs;
using ApiPracticaFinal.Models.DTO.ProyectoDTOs;
using ApiPracticaFinal.Models.DTO.PublicacionDTOs;
using ApiPracticaFinal.Repository.QueryFilters;
using Aspose.Words;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Repository.Proyectos
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly dd4snj9pkf64vpContext context;
        public ProyectoRepository(dd4snj9pkf64vpContext context)
        {
            this.context = context;
        }

        public async Task<bool> Create(ProyectoInsert proyecto)
        {
            Proyecto pro = new Proyecto();

            pro.Activo = true;
            pro.AnioFinalizacion = proyecto.AnioFinalizacion;
            pro.AnioInicio = proyecto.AnioInicio;
            //pro.SsmaTimestamp = new byte[5];
            //pro.MontoContrato = proyecto.MontoContrato;
            //ver de agregar despues
            //pro.Monto = proyecto.Monto;
            pro.NroContrato = proyecto.NroContrato;
            pro.PaisRegion = proyecto.PaisRegion;
            pro.Titulo = proyecto.Titulo;
            pro.Certconformidad = proyecto.Certconformidad;
            pro.Certificadopor = proyecto.Certificadopor;
            //pro.Moneda = proyecto.Moneda;
            pro.EnCurso = proyecto.EnCurso;
            pro.Descripcion = proyecto.Descripcion;
            pro.Departamento = proyecto.Departamento;
            pro.ConsultoresAsoc = proyecto.ConsultoresAsoc;
            pro.Resultados = proyecto.Resultados;
            pro.MesInicio = proyecto.MesInicio;
            pro.MesFinalizacion = proyecto.MesFinalizacion;
            pro.Contratante = proyecto.Contratante;
            pro.Dirección = proyecto.Dirección;
            pro.FichaLista = proyecto.FichaLista;
            pro.Link = proyecto.Link;
            pro.Convenio = proyecto.Convenio;

            await context.Proyectos.AddAsync(pro);
            var valor = await context.SaveChangesAsync();

            if (valor == 0)
            {
                throw new Exception("No se inserto el proyecto");
            }

            foreach (var i in proyecto.ListaAreas)
            {
                Areasxproyecto area = new Areasxproyecto();
                area.Idarea = i.IdArea;
                area.Idproyecto = pro.Id;
                await context.Areasxproyectos.AddAsync(area);
            }

            foreach (var j in proyecto.ListaPersonal)
            {
                Equipoxproyecto equipo = new Equipoxproyecto();
                equipo.IdPersonal = j.IdPersonal;
                equipo.Coordinador = j.Coordinador;
                equipo.IdProyecto = pro.Id;
                equipo.SsmaTimestamp = new byte[5];

                await context.Equipoxproyectos.AddAsync(equipo);
            }

            foreach (var p in proyecto.ListaPublicaciones)
            {
                Publicacionesxproyecto publi = new Publicacionesxproyecto();
                publi.Año = p.Año;
                publi.Codigobcs = p.Codigobcs;
                publi.IdProyecto = pro.Id;
                publi.Publicacion = p.Publicacion;

                await context.Publicacionesxproyectos.AddAsync(publi);
            }

            foreach (var v in proyecto.ListaPresupuestos)
            {
                Presupuesto presupuesto = new Presupuesto();
                presupuesto.Equipamiento = v.Equipamiento;
                presupuesto.Gastos = v.Gastos;
                presupuesto.Viatico = v.Viatico;
                presupuesto.Honorario = v.Honorario;
                presupuesto.Idproyecto = pro.Id;
                presupuesto.Divisa = v.Divisa;
                presupuesto.Montototal = v.Montototal;

                await context.Presupuestos.AddAsync(presupuesto);
            }

            valor = await context.SaveChangesAsync();

            if (valor == 0)
            {
                throw new Exception("No se pudo insertar el proyecto con area y/o personal y/o presupuesto y/o publicaciones");
            }
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var proyecto = await context.Proyectos.FirstOrDefaultAsync(x => x.Id == id);
            if (proyecto == null)
            {
                throw new Exception("No se econtro el proyecto");
            }

            proyecto.Activo = false;
            await context.SaveChangesAsync();
            return true;
        }

        public List<ProyectoTablaDTO> GetListaProyectos(ProyectoQueryFilters filters)
        {
            var proyectos = context.Proyectos.Where(i => i.Activo.Equals(true)).OrderByDescending(x => x.Id).ToList();
            var areasxproyectosBD = context.Areasxproyectos.ToList();
            var equipoxproyectoBD = context.Equipoxproyectos.ToList();
            var presupuestosxproyectoBD = context.Presupuestos.ToList();

            var areaBD = context.Areas.ToList();
            var equipoBD = context.Personals.ToList();

            var equipoxproyecto = new List<Equipoxproyecto>();
            var presupuestos2 = new Presupuesto();

            var listProyectoDto = new List<ProyectoTablaDTO>();

            string fichaLista = "";

            foreach (var i in proyectos)
            {
                //string fichaLista = "";
                //var areaxProyecto = await context.Areasxproyectos.Where(x => x.Idproyecto == i.Id).ToListAsync();
                var areaxProyecto = areasxproyectosBD.Where(x => x.Idproyecto == i.Id).ToList();

                var listaAreaDto = new List<AreaDTO>();

                foreach (var j in areaxProyecto)
                {
                    //var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == j.Idarea);
                    var area = areaBD.FirstOrDefault(x => x.Id == j.Idarea);

                    if (area != null)
                    {
                        var areaDto = new AreaDTO
                        {
                            Id = area.Id,
                            Area1 = area.Area1
                        };
                        listaAreaDto.Add(areaDto);
                    }
                }

                equipoxproyecto = equipoxproyectoBD.Where(x => x.IdProyecto == i.Id).ToList();
                var listaPersonal = new List<PersonalDTO>();

                foreach (var item in equipoxproyecto)
                {
                    var persona = equipoBD.FirstOrDefault(x => x.Id == item.IdPersonal);
                    if (persona != null)
                    {
                        var personaDTO = new PersonalDTO
                        {
                            Nombre = persona.Nombre,
                            Coordinador = item.Coordinador
                        };
                        listaPersonal.Add(personaDTO);
                    }
                }

                var presupuestosBD = presupuestosxproyectoBD.Where(x => x.Idproyecto == i.Id).ToList();
                var listaPresupuestos = new List<PresupuestoDTO>();

                foreach (var item2 in presupuestosBD)
                {
                    var prespuestoDTO = new PresupuestoDTO
                    {
                        Divisa = item2.Divisa,
                        Equipamiento = item2.Equipamiento,
                        Gastos = item2.Gastos,
                        Honorario = item2.Honorario,
                        Montototal = item2.Montototal,
                        Viatico = item2.Viatico
                    };
                    listaPresupuestos.Add(prespuestoDTO);
                }

                if (i.FichaLista == true)
                {
                    fichaLista = "Ficha Lista";
                }
                else
                {
                    fichaLista = "Ficha no Lista";
                }

                var proyectoDto = new ProyectoTablaDTO
                {
                    Id = i.Id,
                    Titulo = i.Titulo,
                    AnioFinalizacion = i.AnioFinalizacion,
                    AnioInicio = i.AnioInicio,
                    Departamento = i.Departamento,
                    ListaAreas = listaAreaDto,
                    FichaLista = i.FichaLista,
                    Ficha = fichaLista,
                    MesFinalizacion = i.MesFinalizacion,
                    MesInicio = i.MesInicio,
                    PaisRegion = i.PaisRegion,
                    MontoContrato = i.MontoContrato,
                    Moneda = i.Moneda,
                    Descripcion = i.Descripcion,
                    Consultores = i.ConsultoresAsoc,
                    Contratante = i.Contratante,
                    ListaPersonal = listaPersonal,
                    ListaPresupuestos = listaPresupuestos
                };

                listProyectoDto.Add(proyectoDto);
            }

            if (filters.Area != null)
            {
                areasxproyectosBD = areasxproyectosBD.Where(x => x.Idarea == filters.Area).ToList();

                var listaProyectoDtoFilter = new List<ProyectoTablaDTO>();

                Proyecto p = null;

                foreach (var j in areasxproyectosBD)
                {
                    var listAreaDtoFilter = new List<AreaDTO>();
                    var areas = new List<Area>();

                    
                    var personales = new List<Personal>();

                    
                    var presupuestos = new List<Presupuesto>();

                    p = context.Proyectos.FirstOrDefault(x => x.Id == j.Idproyecto && x.Activo == true);
                    //var listaArea = await context.Areas.Where(x => x.Id == j.Idarea).ToListAsync();

                    if (p != null)
                    {
                        foreach (var k in p.Areasxproyectos)
                        {
                            var area = context.Areas.FirstOrDefault(x => x.Id == k.Idarea);
                            areas.Add(area);
                        }

                        foreach (var l in areas)
                        {
                            var areaDTO = new AreaDTO
                            {
                                Id = l.Id,
                                Area1 = l.Area1
                            };
                            listAreaDtoFilter.Add(areaDTO);
                        }

                        if (p.FichaLista == true)
                        {
                            fichaLista = "Ficha Lista";
                        }
                        else
                        {
                            fichaLista = "Ficha no Lista";
                        }

                        var pDto = new ProyectoTablaDTO
                        {
                            Id = p.Id,
                            Titulo = p.Titulo,
                            AnioFinalizacion = p.AnioFinalizacion,
                            AnioInicio = p.AnioInicio,
                            Departamento = p.Departamento,
                            ListaAreas = listAreaDtoFilter,
                            FichaLista = p.FichaLista,
                            Ficha = fichaLista,
                            MesFinalizacion = p.MesFinalizacion,
                            MesInicio = p.MesInicio,
                            PaisRegion = p.PaisRegion,
                            MontoContrato = p.MontoContrato,
                            Moneda = p.Moneda,
                            Descripcion = p.Descripcion,
                            Consultores = p.ConsultoresAsoc,
                            Contratante = p.Contratante
                        };

                        listaProyectoDtoFilter.Add(pDto);
                        areas.Clear();

                        equipoxproyecto = equipoxproyectoBD.Where(x => x.IdProyecto == p.Id).ToList();
                        var listaPersonalFilter = new List<PersonalDTO>();

                        foreach (var l in equipoxproyecto)
                        {
                            //var personal2 = context.Personals.FirstOrDefault(x => x.Id == l.IdPersonal);
                            var personal2 = equipoBD.FirstOrDefault(x => x.Id == l.IdPersonal);

                            if (personal2 != null)
                            {
                                personales.Add(personal2);

                                foreach (var item2 in personales)
                                {
                                    var personalDto = new PersonalDTO
                                    {
                                        Nombre = item2.Nombre,
                                        Coordinador = l.Coordinador
                                    };
                                    listaPersonalFilter.Add(personalDto);
                                }
                                personales.Clear();
                            }
                        }
                        pDto.ListaPersonal = listaPersonalFilter;

                        presupuestos2 = context.Presupuestos.FirstOrDefault(x => x.Idproyecto == p.Id);
                        var listaPresupuestosFilter = new List<PresupuestoDTO>();

                        if(presupuestos2 == null)
                        {
                            pDto.ListaPresupuestos = listaPresupuestosFilter;
                        }
                        else
                        {
                            presupuestos.Add(presupuestos2);

                            foreach (var item3 in presupuestos)
                            {
                                var presupuestoDTO = new PresupuestoDTO
                                {
                                    Divisa = item3.Divisa,
                                    Equipamiento = item3.Equipamiento,
                                    Gastos = item3.Gastos,
                                    Honorario = item3.Honorario,
                                    Montototal = item3.Montototal,
                                    Viatico = item3.Viatico
                                };
                                listaPresupuestosFilter.Add(presupuestoDTO);
                            }
                            presupuestos.Clear();

                            
                            pDto.ListaPresupuestos = listaPresupuestosFilter;
                        }
                        
                    }

                }
                listProyectoDto = listaProyectoDtoFilter;

            }

            if (filters.Id != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.Id == filters.Id).ToList();
            }
            if (filters.AnioInicio != null && filters.AnioInicio != null && filters.AnioInicio <= filters.AnioFin)
            {
                listProyectoDto = listProyectoDto.Where(x => x.AnioInicio >= filters.AnioInicio && x.AnioFinalizacion <= filters.AnioFin).ToList();
            }
            if (filters.Pais != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.PaisRegion.ToLower() == filters.Pais).ToList();
            }
            if (filters.Departamento != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.Departamento == filters.Departamento).ToList();
            }
            return listProyectoDto;
        }

        public async Task<List<ProyectoDTO>> GetProyectoId(int id)
        {
            var pro = await context.Proyectos.FirstOrDefaultAsync(x => x.Id == id && x.Activo == true);
            var areasxproyectosBD = await context.Areasxproyectos.ToListAsync();
            var publixproyectoBD = await context.Publicacionesxproyectos.ToListAsync();
            var presupuestoxproyectoBD = await context.Presupuestos.ToListAsync();
            var equipoxproyectoBD = await context.Equipoxproyectos.ToListAsync();
            var areaBD = await context.Areas.ToListAsync();
            var personalBD = await context.Personals.ToListAsync();

            var listaProyectoDto = new List<ProyectoDTO>();

            if (pro == null)
            {
                throw new Exception("Proyecto no encontrado");
            }

            var listaAreaDto = new List<AreaDTO>();
            var listaPublicacionesDto = new List<PublicacionDTO>();
            var listaPrespuestoDto = new List<PresupuestoDTO>();
            var listaPersonaDto = new List<PersonalGetIdDTO>();

            var areaxProyecto = areasxproyectosBD.Where(x => x.Idproyecto == id).ToList();
            var publixproyecto = publixproyectoBD.Where(x => x.IdProyecto == id).ToList();
            var presupuestoxproyecto = presupuestoxproyectoBD.Where(x => x.Idproyecto == id).ToList();
            var personalxproyecto = equipoxproyectoBD.Where(x => x.IdProyecto == id).ToList();


            foreach (var j in areaxProyecto)
            {
                //var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == j.Idarea);
                var area = areaBD.FirstOrDefault(x => x.Id == j.Idarea);

                if (area != null)
                {
                    var areaDto = new AreaDTO
                    {
                        Id = area.Id,
                        Area1 = area.Area1
                    };
                    listaAreaDto.Add(areaDto);
                }
            }

            foreach (var p in publixproyecto)
            {
                var publicacion = publixproyectoBD.FirstOrDefault(x => x.IdPublicacion == p.IdPublicacion);
                if (publicacion != null)
                {
                    var publiDTO = new PublicacionDTO
                    {
                        IdPublicacion = publicacion.IdPublicacion,
                        IdProyecto = publicacion.IdProyecto,
                        Año = publicacion.Año,
                        Codigobcs = publicacion.Codigobcs,
                        Publicacion = publicacion.Publicacion
                    };
                    listaPublicacionesDto.Add(publiDTO);
                }
            }

            foreach (var item in presupuestoxproyecto)
            {
                var presupuesto = presupuestoxproyectoBD.FirstOrDefault(x => x.Idpresupuesto == item.Idpresupuesto);
                if (presupuesto != null)
                {
                    var presupestoDto = new PresupuestoDTO
                    {
                        Idpresupuesto = item.Idpresupuesto,
                        Idproyecto = item.Idproyecto,
                        Honorario = item.Honorario,
                        Equipamiento = item.Equipamiento,
                        Montototal = item.Montototal,
                        Gastos = item.Gastos,
                        Divisa = item.Divisa,
                        Viatico = item.Viatico
                    };
                    listaPrespuestoDto.Add(presupestoDto);
                }
            }

            foreach (var e in personalxproyecto)
            {
                var personal = personalBD.FirstOrDefault(x => x.Id == e.IdPersonal);
                if (personal != null)
                {
                    var personalDTO = new PersonalGetIdDTO
                    {
                        IdPersonal = e.IdPersonal,
                        Nombre = personal.Nombre,
                        Coordinador = e.Coordinador
                    };
                    listaPersonaDto.Add(personalDTO);
                }
            }

            var proyectoDto = new ProyectoDTO
            {
                Id = pro.Id,
                Titulo = pro.Titulo,
                AnioFinalizacion = pro.AnioFinalizacion,
                AnioInicio = pro.AnioInicio,
                Departamento = pro.Departamento,
                ListaAreas = listaAreaDto,
                FichaLista = pro.FichaLista,
                MesFinalizacion = pro.MesFinalizacion,
                MesInicio = pro.MesInicio,
                PaisRegion = pro.PaisRegion,
                MontoContrato = pro.MontoContrato,
                //Moneda = pro.Moneda,
                Link = pro.Link,
                ListaPublicaciones = listaPublicacionesDto,
                ListaPresupuestos = listaPrespuestoDto,
                ListaPersonal = listaPersonaDto,
                Activo = pro.Activo,
                Certconformidad = pro.Certconformidad,
                Certificadopor = pro.Certificadopor,
                ConsultoresAsoc = pro.ConsultoresAsoc,
                Contratante = pro.Contratante,
                Convenio = pro.Convenio,
                Descripcion = pro.Descripcion,
                Dirección = pro.Dirección,
                EnCurso = pro.EnCurso,
                //Monto = pro.Monto,
                NroContrato = pro.NroContrato
            };
            listaProyectoDto.Add(proyectoDto);
            return listaProyectoDto;

        }

        public async Task<List<ProyectoDTO>> GetProyectos()
        {
            var proyectos = await context.Proyectos.Where(i => i.Activo.Equals(true)).OrderByDescending(x => x.Id).ToListAsync();
            var areasxproyectosBD = await context.Areasxproyectos.ToListAsync();
            var publixproyectoBD = await context.Publicacionesxproyectos.ToListAsync();
            var presupuestoxproyectoBD = await context.Presupuestos.ToListAsync();
            var areaBD = await context.Areas.ToListAsync();

            var listProyectoDto = new List<ProyectoDTO>();

            foreach (var i in proyectos)
            {
                //var areaxProyecto = await context.Areasxproyectos.Where(x => x.Idproyecto == i.Id).ToListAsync();
                var areaxProyecto = areasxproyectosBD.Where(x => x.Idproyecto == i.Id).ToList();
                var publixproyecto = publixproyectoBD.Where(x => x.IdProyecto == i.Id).ToList();
                var presupuestoxproyecto = presupuestoxproyectoBD.Where(x => x.Idproyecto == i.Id).ToList();

                var listaAreaDto = new List<AreaDTO>();
                var listaPublicacionesDto = new List<PublicacionDTO>();
                var listaPrespuestoDto = new List<PresupuestoDTO>();

                foreach (var j in areaxProyecto)
                {
                    //var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == j.Idarea);
                    var area = areaBD.FirstOrDefault(x => x.Id == j.Idarea);

                    if (area != null)
                    {
                        var areaDto = new AreaDTO
                        {
                            Id = area.Id,
                            Area1 = area.Area1
                        };
                        listaAreaDto.Add(areaDto);
                    }
                }

                foreach (var p in publixproyecto)
                {
                    var publicacion = publixproyectoBD.FirstOrDefault(x => x.IdPublicacion == p.IdPublicacion);
                    if (publicacion != null)
                    {
                        var publiDTO = new PublicacionDTO
                        {
                            IdPublicacion = publicacion.IdPublicacion,
                            IdProyecto = publicacion.IdProyecto,
                            Año = publicacion.Año,
                            Codigobcs = publicacion.Codigobcs,
                            Publicacion = publicacion.Publicacion
                        };
                        listaPublicacionesDto.Add(publiDTO);
                    }
                }

                foreach (var item in presupuestoxproyecto)
                {
                    var presupuesto = presupuestoxproyectoBD.FirstOrDefault(x => x.Idpresupuesto == item.Idpresupuesto);
                    if (presupuesto != null)
                    {
                        var presupestoDto = new PresupuestoDTO
                        {
                            Idpresupuesto = item.Idpresupuesto,
                            Idproyecto = item.Idproyecto,
                            Honorario = item.Honorario,
                            Equipamiento = item.Equipamiento,
                            Gastos = item.Gastos,
                            Viatico = item.Viatico
                        };
                        listaPrespuestoDto.Add(presupestoDto);
                    }
                }
                var proyectoDto = new ProyectoDTO
                {
                    Id = i.Id,
                    Titulo = i.Titulo,
                    AnioFinalizacion = i.AnioFinalizacion,
                    AnioInicio = i.AnioInicio,
                    Departamento = i.Departamento,
                    ListaAreas = listaAreaDto,
                    FichaLista = i.FichaLista,
                    MesFinalizacion = i.MesFinalizacion,
                    MesInicio = i.MesInicio,
                    PaisRegion = i.PaisRegion,
                    MontoContrato = i.MontoContrato,
                    Moneda = i.Moneda,
                    Link = i.Link,
                    ListaPublicaciones = listaPublicacionesDto,
                    ListaPresupuestos = listaPrespuestoDto
                };

                listProyectoDto.Add(proyectoDto);
            }
            return listProyectoDto;
        }

        public async Task<List<ProyectoTablaDTO>> GetProyectoTabla(ProyectoQueryFilters filters)
        {
            var proyectos = await context.Proyectos.Where(i => i.Activo.Equals(true)).OrderByDescending(x => x.Id).ToListAsync();
            var areasxproyectosBD = await context.Areasxproyectos.ToListAsync();
            var areaBD = await context.Areas.ToListAsync();

            var listProyectoDto = new List<ProyectoTablaDTO>();

            foreach (var i in proyectos)
            {
                string fichaLista = "";
                //var areaxProyecto = await context.Areasxproyectos.Where(x => x.Idproyecto == i.Id).ToListAsync();
                var areaxProyecto = areasxproyectosBD.Where(x => x.Idproyecto == i.Id).ToList();

                var listaAreaDto = new List<AreaDTO>();

                foreach (var j in areaxProyecto)
                {
                    //var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == j.Idarea);
                    var area = areaBD.FirstOrDefault(x => x.Id == j.Idarea);

                    if (area != null)
                    {
                        var areaDto = new AreaDTO
                        {
                            Id = area.Id,
                            Area1 = area.Area1
                        };
                        listaAreaDto.Add(areaDto);
                    }
                }

                if (i.FichaLista == true)
                {
                    fichaLista = "Ficha Lista";
                }
                else
                {
                    fichaLista = "Ficha no Lista";
                }

                var proyectoDto = new ProyectoTablaDTO
                {
                    Id = i.Id,
                    Titulo = i.Titulo,
                    AnioFinalizacion = i.AnioFinalizacion,
                    AnioInicio = i.AnioInicio,
                    Departamento = i.Departamento,
                    ListaAreas = listaAreaDto,
                    FichaLista = i.FichaLista,
                    Ficha = fichaLista,
                    MesFinalizacion = i.MesFinalizacion,
                    MesInicio = i.MesInicio,
                    PaisRegion = i.PaisRegion,
                    MontoContrato = i.MontoContrato,
                    Moneda = i.Moneda,
                };

                listProyectoDto.Add(proyectoDto);
            }

            if (filters.Area != null)
            {
                areasxproyectosBD = areasxproyectosBD.Where(x => x.Idarea == filters.Area).ToList();

                var listaProyectoDtoFilter = new List<ProyectoTablaDTO>();

                Proyecto p = null;

                foreach (var j in areasxproyectosBD)
                {
                    var listAreaDtoFilter = new List<AreaDTO>();
                    var areas = new List<Area>();
                    p = await context.Proyectos.FirstOrDefaultAsync(x => x.Id == j.Idproyecto && x.Activo == true);
                    //var listaArea = await context.Areas.Where(x => x.Id == j.Idarea).ToListAsync();

                    if (p != null)
                    {
                        foreach (var k in p.Areasxproyectos)
                        {
                            var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == k.Idarea);
                            areas.Add(area);
                        }

                        foreach (var l in areas)
                        {
                            var areaDTO = new AreaDTO
                            {
                                Id = l.Id,
                                Area1 = l.Area1
                            };
                            listAreaDtoFilter.Add(areaDTO);
                        }

                        var pDto = new ProyectoTablaDTO
                        {
                            Id = p.Id,
                            Titulo = p.Titulo,
                            AnioFinalizacion = p.AnioFinalizacion,
                            AnioInicio = p.AnioInicio,
                            Departamento = p.Departamento,
                            ListaAreas = listAreaDtoFilter,
                            FichaLista = p.FichaLista,
                            MesFinalizacion = p.MesFinalizacion,
                            MesInicio = p.MesInicio,
                            PaisRegion = p.PaisRegion,
                            MontoContrato = p.MontoContrato,
                            Moneda = p.Moneda,
                        };
                        //listProyectoDto.Add(pDto);
                        listaProyectoDtoFilter.Add(pDto);
                        areas.Clear();
                    }

                }

                listProyectoDto = listaProyectoDtoFilter;
            }

            if (filters.Id != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.Id == filters.Id).ToList();
            }
            if (filters.AnioInicio != null && filters.AnioInicio != null && filters.AnioInicio <= filters.AnioFin)
            {
                listProyectoDto = listProyectoDto.Where(x => x.AnioInicio >= filters.AnioInicio && x.AnioFinalizacion <= filters.AnioFin).ToList();
            }
            if (filters.Pais != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.PaisRegion.ToLower() == filters.Pais).ToList();
            }
            if (filters.Departamento != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.Departamento == filters.Departamento).ToList();
            }
            return listProyectoDto;
        }

        public bool Impresion(ProyectoQueryFilters filters)
        {
            var lista = GetListaProyectos(filters);

            string direccion = "C:\\Users\\Win\\Downloads\\";
            Aspose.Words.Document doc = new Aspose.Words.Document();


            DocumentBuilder builder = new DocumentBuilder(doc);

            string cuerpo = "";
            foreach (var i in lista)
            {
                cuerpo = $"<ul><li>Título: {i.Titulo} </li><li>País/Regíon: {i.PaisRegion} </li><li>Departamento: {i.Departamento} </li>" +
                    $"<li>Ficha: {i.Ficha} </li><li>Año inicio: {i.AnioInicio} </li><li>Año finalización: {i.AnioFinalizacion} </li>" +
                    $"<li> Áreas <ul>";

                if (i.ListaAreas.Count == 0)
                {
                    cuerpo += $"<strong>Sin Áreas asignadas</strong>";
                }
                foreach (var x in i.ListaAreas)
                {
                    cuerpo += $"<li> {x.Area1} </li>";
                }

                cuerpo += "</ul></li><li> Personal <ul>";
                if (i.ListaPersonal.Count == 0)
                {
                    cuerpo += $"<strong>Sin Personal asignado</strong>";
                }
                foreach (var j in i.ListaPersonal)
                {
                    cuerpo += $"<li> {j.Nombre} - {j.Coordinador} </li>";
                }

                cuerpo += "</ul></li></ul>";
                cuerpo += "<hr>";
                builder.InsertHtml(cuerpo);
            }

            var documento = doc.Save("reporte.docx", SaveFormat.Docx);
            if (documento != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<PresupuestoxProyecto>> ListaPresupuestoxProyecto(PresupuestoFilters filters)
        {
            var proyectos = await context.Proyectos.Where(x => x.Activo == true).ToListAsync();
            var presupestoxProyectoBD = await context.Presupuestos.ToListAsync();

            var listaPresupuestoxProyecto = new List<PresupuestoxProyecto>();

            foreach (var i in proyectos)
            {
                var presupuestos = presupestoxProyectoBD.Where(x => x.Idproyecto == i.Id).ToList();

                foreach (var j in presupuestos)
                {
                    var presupuestoxProyecto = new PresupuestoxProyecto
                    {
                        Divisa = j.Divisa,
                        Honorario = j.Honorario,
                        Equipamiento = j.Equipamiento,
                        Gastos = j.Gastos,
                        Montototal = j.Montototal,
                        Viatico = j.Viatico,
                        Titulo = i.Titulo,
                        Anioinicio = i.AnioInicio,
                        AnioFin = i.AnioFinalizacion,
                        Depto = i.Departamento
                    };
                    listaPresupuestoxProyecto.Add(presupuestoxProyecto);
                }
            }

            if(filters.Depto != null)
            {
                listaPresupuestoxProyecto = listaPresupuestoxProyecto.Where(x => x.Depto == filters.Depto).ToList();
            }

            if (filters.AnioDesde != 0 && filters.AnioFin != 0)
            {
                listaPresupuestoxProyecto = listaPresupuestoxProyecto.Where(x => x.Anioinicio >= filters.AnioDesde
                    && x.AnioFin <= filters.AnioFin).ToList();
            }

            return listaPresupuestoxProyecto;
        }

        public async Task<bool> Update(ProyectoUpdate proyecto)
        {
            var pro = await context.Proyectos.FirstOrDefaultAsync(x => x.Id == proyecto.IdProyecto);
            if (pro == null)
            {
                throw new Exception("Proyecto no encontrado");
            }

            pro.Activo = proyecto.Activo ?? pro.Activo;
            pro.AnioFinalizacion = proyecto.AnioFinalizacion ?? pro.AnioFinalizacion;
            pro.AnioInicio = proyecto.AnioInicio ?? pro.AnioInicio;
            //pro.SsmaTimestamp = new byte[5];
            //pro.MontoContrato = proyecto.MontoContrato ?? pro.MontoContrato;
            pro.NroContrato = proyecto.NroContrato ?? pro.NroContrato;
            pro.PaisRegion = proyecto.PaisRegion ?? pro.PaisRegion;
            pro.Titulo = proyecto.Titulo ?? pro.Titulo;
            pro.Certconformidad = proyecto.Certconformidad ?? pro.Certconformidad;
            pro.Certificadopor = proyecto.Certificadopor ?? pro.Certificadopor;
            //pro.Moneda = proyecto.Moneda ?? pro.Moneda;
            pro.EnCurso = proyecto.EnCurso ?? pro.EnCurso;
            pro.Descripcion = proyecto.Descripcion ?? pro.Descripcion;
            pro.Departamento = proyecto.Departamento ?? pro.Departamento;
            pro.ConsultoresAsoc = proyecto.ConsultoresAsoc ?? pro.ConsultoresAsoc;
            pro.Resultados = proyecto.Resultados ?? pro.Resultados;
            pro.MesInicio = proyecto.MesInicio ?? pro.MesInicio;
            pro.MesFinalizacion = proyecto.MesFinalizacion ?? pro.MesFinalizacion;
            pro.Contratante = proyecto.Contratante ?? pro.Contratante;
            pro.Dirección = proyecto.Dirección ?? pro.Dirección;
            pro.FichaLista = proyecto.FichaLista ?? pro.FichaLista;
            pro.Link = proyecto.Link ?? pro.Link;
            pro.Convenio = proyecto.Convenio ?? pro.Convenio;

            var areaxproyecto = await context.Areasxproyectos.Where(x => x.Idproyecto == proyecto.IdProyecto).ToListAsync();
            foreach (var i in areaxproyecto)
            {
                context.Areasxproyectos.Remove(i);
                await context.SaveChangesAsync();
            }

            foreach (var i in proyecto.ListaAreas)
            {
                Areasxproyecto area = new Areasxproyecto();
                area.Idarea = i.IdArea;
                area.Idproyecto = pro.Id;
                await context.Areasxproyectos.AddAsync(area);
            }

            var equipoxproyecto = await context.Equipoxproyectos.Where(x => x.IdProyecto == proyecto.IdProyecto).ToListAsync();
            foreach (var item in equipoxproyecto)
            {
                context.Equipoxproyectos.Remove(item);
                await context.SaveChangesAsync();
            }

            foreach (var j in proyecto.ListaPersonal)
            {
                Equipoxproyecto equipo = new Equipoxproyecto();
                equipo.IdPersonal = j.IdPersonal;
                equipo.Coordinador = j.Coordinador;
                equipo.IdProyecto = pro.Id;
                equipo.SsmaTimestamp = new byte[5];
                await context.Equipoxproyectos.AddAsync(equipo);
            }

            var publicacionesxproyecto = await context.Publicacionesxproyectos.Where(x => x.IdProyecto == proyecto.IdProyecto).ToListAsync();
            foreach (var a in publicacionesxproyecto)
            {
                context.Publicacionesxproyectos.Remove(a);
                await context.SaveChangesAsync();
            }

            foreach (var p in proyecto.ListaPublicaciones)
            {
                Publicacionesxproyecto publi = new Publicacionesxproyecto();
                publi.Año = p.Año;
                publi.Codigobcs = p.Codigobcs;
                publi.IdProyecto = pro.Id;
                publi.Publicacion = p.Publicacion;

                await context.Publicacionesxproyectos.AddAsync(publi);
            }

            var presupuestoxproyecto = await context.Presupuestos.Where(x => x.Idproyecto == proyecto.IdProyecto).ToListAsync();
            foreach (var p in presupuestoxproyecto)
            {
                context.Presupuestos.Remove(p);
                await context.SaveChangesAsync();
            }

            foreach (var v in proyecto.ListaPresupuestos)
            {
                Presupuesto presupuesto = new Presupuesto();
                presupuesto.Equipamiento = v.Equipamiento;
                presupuesto.Gastos = v.Gastos;
                presupuesto.Viatico = v.Viatico;
                presupuesto.Honorario = v.Honorario;
                presupuesto.Idproyecto = pro.Id;
                presupuesto.Divisa = v.Divisa;
                presupuesto.Montototal = v.Montototal;

                await context.Presupuestos.AddAsync(presupuesto);
            }

            context.Proyectos.Update(pro);
            var valor = await context.SaveChangesAsync();

            if (valor == 0)
                throw new Exception("No se pudo actualizar el proyecto");

            return true;
        }
    }
}
