using LN_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LN_WEB.Entities
{
    public class PedidoEnt
    {
        public long OrderId { get; set; }
        public long FacturaId { get; set; }
        public string NombreUsuario { get; set; }
        public decimal Total { get; set; }
        public System.DateTime Fecha { get; set; }
        public long IdUsuario { get; set; }
        public bool ParaLlevar { get; set; }
        public string Nota { get; set; }
        public bool Estado { get; set; }
    }
}