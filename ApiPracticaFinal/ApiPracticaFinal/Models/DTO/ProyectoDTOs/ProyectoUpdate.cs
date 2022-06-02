using ApiPracticaFinal.Models.DTO.Areas;
using ApiPracticaFinal.Models.DTO.PersonalesDTO;
using ApiPracticaFinal.Models.DTO.PresupuestoDTOs;
using ApiPracticaFinal.Models.DTO.PublicacionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPracticaFinal.Models.DTO.ProyectoDTOs
{
    public class ProyectoUpdate
    {
        public int IdProyecto { get; set; }
        public string Titulo { get; set; }
        public string PaisRegion { get; set; }
        public string Contratante { get; set; }
        public string Dirección { get; set; }
        //public string MontoContrato { get; set; }
        public string NroContrato { get; set; }
        public int? MesInicio { get; set; }
        public int? AnioInicio { get; set; }
        public int? MesFinalizacion { get; set; }
        public int? AnioFinalizacion { get; set; }
        public string ConsultoresAsoc { get; set; }
        public string Descripcion { get; set; }
        public string Resultados { get; set; }
        public bool? FichaLista { get; set; }
        public bool? EnCurso { get; set; }
        public string Departamento { get; set; }
        //public string Moneda { get; set; }
        public bool? Certconformidad { get; set; }
        public int? Certificadopor { get; set; }
        public bool? Activo { get; set; }
        public string Link { get; set; }
        public bool? Convenio { get; set; }
        public List<AreaInsertProyecto> ListaAreas { get; set; }
        public List<PresupuestoInsertProyecto> ListaPresupuestos { get; set; }
        public List<PublicacionInsertProyecto> ListaPublicaciones { get; set; }
        public List<PersonalInsertProyecto> ListaPersonal { get; set; }

        //ver de agregar esta propiedad si es necesario
        //public double? Monto { get; set; }
    }
}
