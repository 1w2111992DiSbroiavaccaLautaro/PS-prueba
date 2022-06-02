using System;
using System.Collections.Generic;

#nullable disable

namespace ApiPracticaFinal.Models
{
    public partial class Equipoxproyecto
    {
        public int IdProyecto { get; set; }
        public int IdPersonal { get; set; }
        public string Texto { get; set; }
        public string FuncionTarea { get; set; }
        public bool? Coordinador { get; set; }
        public byte[] SsmaTimestamp { get; set; }
        public char? Trial098 { get; set; }
        public char? Trial236 { get; set; }
        public char? Trial460 { get; set; }

        public virtual Personal IdPersonalNavigation { get; set; }
        public virtual Proyecto IdProyectoNavigation { get; set; }
    }
}
