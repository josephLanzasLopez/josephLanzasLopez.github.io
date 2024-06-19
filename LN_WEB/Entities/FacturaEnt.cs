using System;
using System.Web;

namespace LN_WEB.Entities
{
    public class FacturaEnt
    {

        public long FacturaId { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public long IdUsuario { get; set; }
        public bool ParaLlevar { get; set; }
        public string Nota { get; set; }
    }
}