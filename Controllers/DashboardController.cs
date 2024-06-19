
using LN_WEB.Entities;
using LN_WEB.Models;
using OpenAI_API.Models;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Http.Json;
using System.Collections.Generic;

namespace LN_WEB.Controllers
{
    public class DashboardController : Controller
    {
        DashBoardModel model = new DashBoardModel();

        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }


        [HttpGet]
        public JsonResult listaReporte(string fechainicio, string fechafin, string idFactura)
        {
            List<Reporte> oLista = new List<Reporte>();

            oLista  =  model.ObtenerVentas(fechainicio, fechafin, idFactura);
            
            return Json(new {data = oLista}, JsonRequestBehavior.AllowGet);
        }
    

    [HttpGet]
        public JsonResult VerDashboard()
        {
            var reporteDashboard = model.ObtenerReporteDashboard();
            return Json(reporteDashboard, JsonRequestBehavior.AllowGet);
        }
        }


    
}