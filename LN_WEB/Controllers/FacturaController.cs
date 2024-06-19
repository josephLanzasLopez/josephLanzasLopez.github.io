
using LN_WEB.Entities;
using LN_WEB.Models;
using OpenAI_API.Models;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LN_WEB.Controllers
{
    public class FacturaController : Controller
    {
        FacturaModel model = new FacturaModel();




        [HttpPost]
        public ActionResult CrearFactura(FacturaEnt factura)
        {
            int resultado = model.CrearFactura(factura);

            if (resultado > 0)
            {
                // La factura se creó con éxito
                ViewBag.Mensaje = "Factura creada exitosamente.";
            }
            else
            {
                // Hubo un problema al crear la factura
                ViewBag.Mensaje = "Error al crear la factura.";
            }

            return View();
        }


        public ActionResult VerFacturasUsuario(long userId)
        {
            var facturasUsuario = model.CheckFacturasUsuario(userId);

            if (facturasUsuario != null)
            {
                // Hacer algo con las facturas obtenidas, como pasarlas a una vista
                return View(facturasUsuario);
            }
            else
            {
                // Manejo de errores
                return RedirectToAction("Error");
            }


        }
    }
}