using ApiPracticaFinal.Models;
using ApiPracticaFinal.Models.DTO;
using ApiPracticaFinal.Models.DTO.Reportes;
using ApiPracticaFinal.Repository.QueryFilters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using static ApiPracticaFinal.Models.DTO.CotizacionDTO;

namespace ApiPracticaFinal.Repository.ReportesRepo
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly dd4snj9pkf64vpContext context;
        public ReportesRepository(dd4snj9pkf64vpContext context)
        {
            this.context = context;
        }

        public double dolar = 117.7;
        public decimal dolar2 = 0;
        public double euro = 123.7;

        HttpClient client = new HttpClient();
        public async Task<decimal> GetCotizacion()
        {
            HttpResponseMessage response = await client.GetAsync("https://www.dolarsi.com/api/api.php?type=valoresprincipales");
            if (response.IsSuccessStatusCode)
            {
                var datos = await response.Content.ReadAsStringAsync();
                var json = await response.Content.ReadFromJsonAsync<List<CotizacionDTO>>();
                foreach (var i in json)
                {
                    if (i.casa.nombre == "Dolar Oficial")
                    {
                        dolar2 = Convert.ToDecimal(i.casa.compra);
                        euro = Convert.ToDouble(i.casa.venta);
                    }
                }

                dolar2 = Convert.ToDecimal(dolar2);
                return dolar2;
            }
            return 0;            
        }

        public async Task<Reporte1DTO> Reporte1(Reporte1Filters filters)
        {
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = await client.GetAsync("https://www.dolarsi.com/api/api.php?type=valoresprincipales");
            //if (response.IsSuccessStatusCode)
            //{
            //    var datos = await response.Content.ReadAsStringAsync();
            //    var json = await response.Content.ReadFromJsonAsync<List<CotizacionDTO>>();
            //    foreach (var i in json)
            //    {
            //        if (i.casa.nombre == "Dolar Oficial")
            //        {
            //            dolar = Convert.ToDouble(i.casa.compra);
            //            euro = Convert.ToDouble(i.casa.venta);
            //        }
            //    }
            //}

            //dolar = Convert.ToDouble(dolar);
            //euro = Convert.ToDouble(euro);

            var presupuestos = await context.Presupuestos
                .Include(x => x.IdproyectoNavigation).ToListAsync();

            var proyectos = new List<Proyecto>();
            foreach (var i in presupuestos)
            {
                var proyecto = await context.Proyectos.SingleOrDefaultAsync(x => x.Id == i.Idproyecto);
                proyectos.Add(proyecto);
            }

            double? montoPesos = 0;
            double? montoDol = 0;
            double? montoEuro = 0;
            double? montoTotal = 0;

            double? gastoPesos = 0;
            double? gastoDol = 0;
            double? gastoEuro = 0;
            double? gastoTotal = 0;

            double? montoFinal = 0;
            double? gastoFinal = 0;

            Reporte1DTO reporte = new Reporte1DTO();

            if (filters.Departamento == null && filters.Estado == null)
            {
                proyectos = proyectos.Where(x => x.Activo == true).ToList();
                foreach (var i in proyectos)
                {
                    montoPesos = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                    montoDol = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                    montoEuro = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                    montoTotal = montoPesos + (montoDol * dolar) + (montoEuro * euro);
                    montoFinal = montoFinal + montoTotal;

                    gastoPesos = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Gastos);
                    gastoDol = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Gastos);
                    gastoEuro = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Gastos);
                    gastoTotal = gastoPesos + (gastoDol * dolar) + (gastoEuro * euro);
                    gastoFinal = gastoFinal + gastoTotal;
                }
                reporte.Ganancias = montoFinal;
                reporte.Perdidas = gastoFinal;
            }

            if (filters.Departamento != null && filters.Estado != null)
            {
                proyectos = proyectos.Where(x => x.Activo == true && x.Departamento == filters.Departamento && x.FichaLista == filters.Estado).ToList();

                foreach (var i in proyectos)
                {
                    montoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Montototal);
                    montoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Montototal);
                    montoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Montototal);
                    montoTotal = montoPesos + (montoDol * dolar) + (montoEuro * euro);

                    montoFinal = montoFinal + montoTotal;

                    gastoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Gastos);
                    gastoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Gastos);
                    gastoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Gastos);
                    gastoTotal = gastoPesos + (gastoDol * dolar) + (gastoEuro * euro);

                    gastoFinal = gastoFinal + gastoTotal;
                }
                reporte.Ganancias = montoFinal;
                reporte.Perdidas = gastoFinal;
                
            }
            else if (filters.Estado != null && filters.Departamento == null)
            {
                proyectos = proyectos.Where(x => x.FichaLista == filters.Estado && x.Activo == true).ToList();

                foreach (var i in proyectos)
                {
                    montoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Montototal);
                    montoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Montototal);
                    montoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Montototal);
                    montoTotal = montoPesos + (montoDol * dolar) + (montoEuro * euro);

                    montoFinal = montoFinal + montoTotal;

                    gastoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Gastos);
                    gastoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Gastos);
                    gastoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Gastos);
                    gastoTotal = gastoPesos + (gastoDol * dolar) + (gastoEuro * euro);

                    gastoFinal = gastoFinal + gastoTotal;
                }
                reporte.Ganancias = montoFinal;
                reporte.Perdidas = gastoFinal;
                

            } else if (filters.Departamento != null && filters.Estado == null)
            {
                proyectos = proyectos.Where(x => x.Departamento == filters.Departamento && x.Activo == true).ToList();
                
                foreach (var i in proyectos)
                {
                    montoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Montototal);
                    montoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Montototal);
                    montoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Montototal);
                    montoTotal = montoPesos + (montoDol * dolar) + (montoEuro * euro);

                    montoFinal = montoFinal + montoTotal;

                    gastoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Gastos);
                    gastoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Gastos);
                    gastoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Gastos);
                    gastoTotal = gastoPesos + (gastoDol * dolar) + (gastoEuro * euro);

                    gastoFinal = gastoFinal + gastoTotal;

                }
                reporte.Ganancias = montoFinal;
                reporte.Perdidas = gastoFinal;
                

            }

            return reporte;
        }

        public async Task<Reporte2DTO> Reporte2(Reporte2Filter filters)
        {
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = await client.GetAsync("https://www.dolarsi.com/api/api.php?type=valoresprincipales");
            //if (response.IsSuccessStatusCode)
            //{
            //    var datos = await response.Content.ReadAsStringAsync();
            //    var json = await response.Content.ReadFromJsonAsync<List<CotizacionDTO>>();
            //    foreach (var i in json)
            //    {
            //        if (i.casa.nombre == "Dolar Oficial")
            //        {
            //            dolar = Convert.ToDouble(i.casa.compra);
            //            euro = Convert.ToDouble(i.casa.venta);
            //        }
            //    }
            //}

            var presupuestos = await context.Presupuestos
                .Include(x => x.IdproyectoNavigation).ToListAsync();

            var proyectos = new List<Proyecto>();
            foreach (var i in presupuestos)
            {
                var proyecto = await context.Proyectos.SingleOrDefaultAsync(x => x.Id == i.Idproyecto);
                proyectos.Add(proyecto);
            }

            Reporte2DTO reporte = new Reporte2DTO();

            double? honorarioPesos = 0;
            double? honorarioDol = 0;
            double? honorarioEuro = 0;
            double? honorarioTotal = 0;

            double? ViaticoPesos = 0;
            double? ViaticoDol = 0;
            double? ViaticoEuro = 0;
            double? ViaticoTotal = 0;

            double? equipoPesos = 0;
            double? equipoDol = 0;
            double? equipoEuro = 0;
            double? equipoTotal = 0;

            double? gastoPesos = 0;
            double? gastoDol = 0;
            double? gastoEuro = 0;
            double? gastoTotal = 0;

            double? honorariosFinal = 0;
            double? viaticosFinal = 0;
            double? equipoFinal = 0;
            double? gastosFinal = 0;

            if (filters.AnioDesde == null && filters.AnioHasta == null && filters.Estado == null)
            {
                proyectos = proyectos.Where(x => x.Activo == true).ToList();
                foreach (var i in proyectos)
                {
                    //honorarios
                    honorarioPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Honorario);
                    honorarioDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Honorario);
                    honorarioEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Honorario);
                    honorarioTotal = honorarioPesos + (honorarioDol * dolar) + (honorarioEuro * euro);
                    honorariosFinal = honorariosFinal + honorarioTotal;

                    //viaticos
                    ViaticoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Viatico);
                    ViaticoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Viatico);
                    ViaticoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Viatico);
                    ViaticoTotal = ViaticoPesos + (ViaticoDol * dolar) + (ViaticoEuro * euro);
                    viaticosFinal = viaticosFinal + ViaticoTotal;

                    //equipo
                    equipoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Equipamiento);
                    equipoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Equipamiento);
                    equipoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Equipamiento);
                    equipoTotal = equipoPesos + (equipoDol * dolar) + (equipoEuro * euro);
                    equipoFinal = equipoFinal + equipoTotal;

                    //gastos
                    gastoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Gastos);
                    gastoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Gastos);
                    gastoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Gastos);
                    gastoTotal = gastoPesos + (gastoDol * dolar) + (gastoEuro * euro);
                    gastosFinal = gastosFinal + gastoTotal;
                }
                reporte.Honorarios = honorariosFinal;
                reporte.Viaticos = viaticosFinal;
                reporte.Equipo = equipoFinal;
                reporte.Gastos = gastosFinal;
            }

            if (filters.AnioDesde != null && filters.AnioHasta != null && filters.Estado != null)
            {
                proyectos = proyectos.Where(x => x.Activo == true && x.AnioInicio >= filters.AnioDesde && x.AnioInicio <= filters.AnioHasta && x.FichaLista == filters.Estado).ToList();
                
                foreach (var i in proyectos)
                {
                    //honorarios
                    honorarioPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Honorario);
                    honorarioDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Honorario);
                    honorarioEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Honorario);
                    honorarioTotal = honorarioPesos + (honorarioDol * dolar) + (honorarioEuro * euro);
                    honorariosFinal = honorariosFinal + honorarioTotal;
                
                    //viaticos
                    ViaticoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Viatico);
                    ViaticoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Viatico);
                    ViaticoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Viatico);
                    ViaticoTotal = ViaticoPesos + (ViaticoDol * dolar) + (ViaticoEuro * euro);
                    viaticosFinal = viaticosFinal + ViaticoTotal;
                
                    //equipo
                    equipoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Equipamiento);
                    equipoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Equipamiento);
                    equipoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Equipamiento);
                    equipoTotal = equipoPesos + (equipoDol * dolar) + (equipoEuro * euro);
                    equipoFinal = equipoFinal + equipoTotal;
                
                    //gastos
                    gastoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Gastos);
                    gastoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Gastos);
                    gastoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Gastos);
                    gastoTotal = gastoPesos + (gastoDol * dolar) + (gastoEuro * euro);
                    gastosFinal = gastosFinal + gastoTotal;
                }

                reporte.Viaticos = viaticosFinal;
                reporte.Honorarios = honorariosFinal;
                reporte.Equipo = equipoFinal;
                reporte.Gastos = gastosFinal;
            }

            if (filters.AnioDesde != null && filters.AnioHasta != null && filters.Estado == null)
            {
                proyectos = proyectos.Where(x => x.Activo == true && x.AnioInicio >= filters.AnioDesde && x.AnioInicio <= filters.AnioHasta).ToList();

                foreach (var i in proyectos)
                {
                    //honorarios
                    honorarioPesos = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Honorario);
                    honorarioDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Honorario);
                    honorarioEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Honorario);
                    honorarioTotal = honorarioPesos + (honorarioDol * dolar) + (honorarioEuro * euro);
                    honorariosFinal = honorariosFinal + honorarioTotal;

                    //viaticos
                    ViaticoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Viatico);
                    ViaticoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Viatico);
                    ViaticoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Viatico);
                    ViaticoTotal = ViaticoPesos + (ViaticoDol * dolar) + (ViaticoEuro * euro);
                    viaticosFinal = viaticosFinal + ViaticoTotal;

                    //equipo
                    equipoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Equipamiento);
                    equipoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Equipamiento);
                    equipoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Equipamiento);
                    equipoTotal = equipoPesos + (equipoDol * dolar) + (equipoEuro * euro);
                    equipoFinal = equipoFinal + equipoTotal;

                    //gastos
                    gastoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Gastos);
                    gastoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Gastos);
                    gastoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Gastos);
                    gastoTotal = gastoPesos + (gastoDol * dolar) + (gastoEuro * euro);
                    gastosFinal = gastosFinal + gastoTotal;
                }

                reporte.Viaticos = viaticosFinal;
                reporte.Honorarios = honorariosFinal;
                reporte.Equipo = equipoFinal;
                reporte.Gastos = gastosFinal;
            }

            if (filters.Estado != null && filters.AnioDesde == null && filters.AnioHasta == null)
            {
                proyectos = proyectos.Where(x => x.Activo == true && x.FichaLista == filters.Estado).ToList();

                foreach (var i in proyectos)
                {
                    //honorarios
                    honorarioPesos = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Honorario);
                    honorarioDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Honorario);
                    honorarioEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Honorario);
                    honorarioTotal = honorarioPesos + (honorarioDol * dolar) + (honorarioEuro * euro);
                    honorariosFinal = honorariosFinal + honorarioTotal;

                    //viaticos
                    ViaticoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Viatico);
                    ViaticoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Viatico);
                    ViaticoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Viatico);
                    ViaticoTotal = ViaticoPesos + (ViaticoDol * dolar) + (ViaticoEuro * euro);
                    viaticosFinal = viaticosFinal + ViaticoTotal;

                    //equipo
                    equipoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Equipamiento);
                    equipoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Equipamiento);
                    equipoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Equipamiento);
                    equipoTotal = equipoPesos + (equipoDol * dolar) + (equipoEuro * euro);
                    equipoFinal = equipoFinal + equipoTotal;

                    //gastos
                    gastoPesos = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "PESOS").Sum(x => x.Gastos);
                    gastoDol = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "USD").Sum(x => x.Gastos);
                    gastoEuro = presupuestos.Where(x => x.Idproyecto == i.Id && x.Divisa == "EUR").Sum(x => x.Gastos);
                    gastoTotal = gastoPesos + (gastoDol * dolar) + (gastoEuro * euro);
                    gastosFinal = gastosFinal + gastoTotal;
                }

                reporte.Viaticos = viaticosFinal;
                reporte.Honorarios = honorariosFinal;
                reporte.Equipo = equipoFinal;
                reporte.Gastos = gastosFinal;
            }

            return reporte;
        }

        public async Task<Reporte3DTO> Reporte3(Reporte3Filters filters)
        {
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = await client.GetAsync("https://www.dolarsi.com/api/api.php?type=valoresprincipales");
            //if (response.IsSuccessStatusCode)
            //{
            //    var datos = await response.Content.ReadAsStringAsync();
            //    var json = await response.Content.ReadFromJsonAsync<List<CotizacionDTO>>();
            //    foreach (var i in json)
            //    {
            //        if (i.casa.nombre == "Dolar Oficial")
            //        {
            //            dolar = Convert.ToDouble(i.casa.compra);
            //            euro = Convert.ToDouble(i.casa.venta);
            //        }
            //    }
            //}

            var presupuestos = await context.Presupuestos
                .Include(x => x.IdproyectoNavigation).ToListAsync();

            var proyectos = new List<Proyecto>();
            foreach (var i in presupuestos)
            {
                var proyecto = await context.Proyectos.SingleOrDefaultAsync(x => x.Id == i.Idproyecto);
                proyectos.Add(proyecto);
            }

            Reporte3DTO reporte = new Reporte3DTO();

            //ENERGIA
            double? energiaP = 0;
            double? energiaD = 0;
            double? energiaE = 0;
            double? energiaTotal = 0;
            double? energiaFinal = 0;
            //MADE
            double? madeP = 0;
            double? madeD = 0;
            double? madeE = 0;
            double? madeTotal = 0;
            double? madeFinal = 0;
            //ASC
            double? ascP = 0;
            double? ascD = 0;
            double? ascE = 0;
            double? ascTotal = 0;
            double? ascFinal = 0;

            if (filters.Estado == null && filters.AnioDesde == null && filters.AnioHasta == null)
            {
                proyectos = proyectos.Where(x => x.Activo == true).ToList();

                foreach (var i in proyectos)
                {
                    if (i.Departamento == "ENERGIA" || i.Departamento == "ENERGÍA")
                    {
                        energiaP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaTotal = energiaP + (energiaD * dolar) + (energiaE * euro);
                        energiaFinal = energiaFinal + energiaTotal;
                    }

                    if (i.Departamento == "MADE")
                    {
                        madeP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeTotal = madeP + (madeD * dolar) + (madeE * euro);
                        madeFinal = madeFinal + madeTotal;
                    }

                    if (i.Departamento == "ASC")
                    {
                        ascP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascTotal = ascP + (ascD * dolar) + (ascE * euro);
                        ascFinal = ascFinal + ascTotal;
                    }
                }

                reporte.Energia = energiaFinal;
                reporte.Made = madeFinal;
                reporte.Asc = ascFinal;
            }
            
            if (filters.Estado != null && filters.AnioDesde == null && filters.AnioHasta == null)
            {
                proyectos = proyectos.Where(x => x.Activo == true && x.FichaLista == filters.Estado).ToList();

                foreach (var i in proyectos)
                {
                    if (i.Departamento == "ENERGIA")
                    {
                        energiaP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaTotal = energiaP + (energiaD * dolar) + (energiaE * euro);
                        energiaFinal = energiaFinal + energiaTotal;
                    }

                    if (i.Departamento == "MADE")
                    {
                        madeP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeTotal = madeP + (madeD * dolar) + (madeE * euro);
                        madeFinal = madeFinal + madeTotal;
                    }

                    if (i.Departamento == "ASC")
                    {
                        ascP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascTotal = ascP + (ascD * dolar) + (ascE * euro);
                        ascFinal = ascFinal + ascTotal;
                    }
                }
                reporte.Energia = energiaFinal;
                reporte.Made = madeFinal;
                reporte.Asc = ascFinal;
            }
            if(filters.Estado == null && filters.AnioDesde != null && filters.AnioHasta != null)
            {
                proyectos = proyectos.Where(x => x.Activo == true && x.AnioInicio >= filters.AnioDesde && x.AnioFinalizacion <= filters.AnioHasta).ToList();

                foreach (var i in proyectos)
                {
                    if (i.Departamento == "ENERGIA")
                    {
                        energiaP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaTotal = energiaP + (energiaD * dolar) + (energiaE * euro);
                        energiaFinal = energiaFinal + energiaTotal;
                    }

                    if (i.Departamento == "MADE")
                    {
                        madeP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeTotal = madeP + (madeD * dolar) + (madeE * euro);
                        madeFinal = madeFinal + madeTotal;
                    }

                    if (i.Departamento == "ASC")
                    {
                        ascP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascTotal = ascP + (ascD * dolar) + (ascE * euro);
                        ascFinal = ascFinal + ascTotal;
                    }
                }
                reporte.Energia = energiaFinal;
                reporte.Made = madeFinal;
                reporte.Asc = ascFinal;
            }
            if(filters.Estado != null && filters.AnioDesde != null && filters.AnioHasta != null)
            {
                proyectos = proyectos.Where(x => x.Activo == true && x.FichaLista == filters.Estado && x.AnioInicio >= filters.AnioDesde && x.AnioFinalizacion <= filters.AnioHasta).ToList();

                foreach (var i in proyectos)
                {
                    if (i.Departamento == "ENERGIA")
                    {
                        energiaP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        energiaTotal = energiaP + (energiaD * dolar) + (energiaE * euro);
                        energiaFinal = energiaFinal + energiaTotal;
                    }

                    if (i.Departamento == "MADE")
                    {
                        madeP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        madeTotal = madeP + (madeD * dolar) + (madeE * euro);
                        madeFinal = madeFinal + madeTotal;
                    }

                    if (i.Departamento == "ASC")
                    {
                        ascP = presupuestos.Where(x => x.Divisa == "PESOS" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascD = presupuestos.Where(x => x.Divisa == "USD" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascE = presupuestos.Where(x => x.Divisa == "EUR" && x.Idproyecto == i.Id).Sum(x => x.Montototal);
                        ascTotal = ascP + (ascD * dolar) + (ascE * euro);
                        ascFinal = ascFinal + ascTotal;
                    }
                }
                reporte.Energia = energiaFinal;
                reporte.Made = madeFinal;
                reporte.Asc = ascFinal;
            }

            return reporte;
        }

        public async Task<ProyectosxDepto> ProyectosxDepto(ValidadorFilter filter)
        {
            var proyectos = new List<Proyecto>();
            if (filter.AnioDesde != 0 && filter.AnioFin != 0)
            {
                proyectos = await context.Proyectos.Where
                    (x => x.Activo == true && x.AnioInicio >= filter.AnioDesde && x.AnioFinalizacion <= filter.AnioFin).ToListAsync();
            }
            else
            {
                proyectos = await context.Proyectos.Where(x => x.Activo == true).ToListAsync();
            }

            var energia = proyectos.Where(x => x.Departamento == "ENERGIA").Count();
            var made = proyectos.Where(x => x.Departamento == "MADE").Count();
            var asc = proyectos.Where(x => x.Departamento == "ASC").Count();

            var proyectoxDepto = new ProyectosxDepto
            {
                Energia = energia,
                Made = made,
                Asc = asc
            };

            return proyectoxDepto;
        }
    }
}
