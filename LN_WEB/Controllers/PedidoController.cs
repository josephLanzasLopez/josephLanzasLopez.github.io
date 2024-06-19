using LN_WEB.Entities;
using LN_WEB.Models;
using LN_WEB.Models.Paypal_Transaction;
using Newtonsoft.Json;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LN_WEB.Controllers
{
    public class PedidoController : Controller
    {
        PedidoModel model = new PedidoModel();


        [HttpPost]
        public ActionResult RemoverPedido(long q)
        {
            var resp = model.RemoverPedido(q);
            ActualizarTotales();

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        private void ActualizarTotales()
        {
            // Verificar si la sesión tiene un valor para "OrderId"
            var orderIdSession = Session["OrderId"] as string;

            if (!string.IsNullOrEmpty(orderIdSession))
            {
                var pedidoActual = model.CheckPedido(long.Parse(orderIdSession));

                // Resto del código para actualizar totales...
            }
            else
            {
                // Manejar el caso cuando "OrderId" no está en la sesión
                ViewBag.ErrorMessage = "No se encontró el ID del pedido en la sesión.";
            }
        }

        [HttpPost]
        public ActionResult EditarPedido(PedidoEnt entidad)
        {
            var resp = model.EditarPedido(entidad);

            if (resp > 0)
                return RedirectToAction("CheckPedidos", "Pedido");
            else
            {
                ViewBag.MsjPantalla = "No se ha podido actualizar la información del pedido";
                return View("CheckPedidos");
            }
        }

        [HttpGet]
        public ActionResult Editar(long q)
        {
            var resp = model.CheckPedido(q);
            return View(resp);
        }


        [HttpGet]
        public ActionResult CheckPedidos()
        {
            var datos = model.CheckPedidos();
            return View(datos);
        }


        [HttpGet]
        public ActionResult CambiarEstado(long q)
        {
            PedidoEnt entidad = new PedidoEnt();
            entidad.OrderId = q;

            var resp = model.CambiarEstado(entidad);

            if (resp > 0)
                return RedirectToAction("CheckPedidos", "Pedido");
            else
            {
                ViewBag.MsjPantalla = "No se ha podido actualizar el estado del pedido";
                return View("CheckPedidos");
            }
        }

    }
}