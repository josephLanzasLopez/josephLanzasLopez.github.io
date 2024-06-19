using LN_WEB.Entities;
using LN_WEB.Models;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace LN_WEB.Controllers
{
    public class ReservationController : Controller
    {
        ReservationModel model = new ReservationModel();



        //[HttpGet]
        //public ActionResult CheckMyReservations()
        //{
        //    var datos = model.CheckMyReservations(long.Parse(Session["IdUsuario"].ToString()));
        //    return View(datos);
        //}



        [HttpGet]
        public ActionResult AddReservation()
        {

            return View();
        }



        [HttpPost]
        public ActionResult RemoveReservation(long q)
        {
            var resp = model.RemoveReservation(q);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> ObtenerReservaciones()
        {
            // Obtener las reservaciones de manera asíncrona
            var reservaciones = await model.CheckReservationsDatesAsync();

            // Procesar las reservaciones para obtener las fechas en el formato deseado
            var citas = reservaciones.Select(reserva => new
            {
                title = "Reservado",
                start = reserva.DateReservation.ToString("yyyy-MM-dd"), // Formatear la fecha según tu lógica
                color = "blue"
            });

            // Devolver el resultado como un JsonResult
            return Json(citas, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult listAgendas(string fechainicio, string fechafin, string idReserva)
        {
            List<Reservados> oLista = new List<Reservados>();

            oLista = model.ReservasAgendadas(fechainicio, fechafin, idReserva);

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public ActionResult CheckReservations()
        {
            return View();
        }

        // GET: Reservations/Create
        public ActionResult CheckMyReservations()
        {
            // Aquí podrías agregar lógica para cargar datos necesarios en la vista de creación de reservas
            return View();
        }

        // POST: Reservations/CheckMyReservations
        [HttpPost]
        public ActionResult CheckMyReservations(ReservationEnt reservation)
        {
            if (ModelState.IsValid)
            {
                // Lógica para registrar una nueva reserva
                model.RegisterReservation(reservation);

                ViewBag.GoodMsjPantalla = "Reservacion agregada correctamente";
                return RedirectToAction("AddReservation");
            }
            // Si hay errores de validación, regresamos a la vista de creación con los mensajes de error
            return View(reservation);
        }

        [HttpPost]
        public ActionResult RegisterReservation(ReservationEnt entidad)
        {

            try
            {
                // Validaciones del modelo
                if (ModelState.IsValid)
                {

                    entidad.State = true;

                    var resp = model.RegisterReservation(entidad);

                    if (resp > 0)
                    {
                        return RedirectToAction("CheckMyReservations", "Reservation");
                    }
                    else
                    {
                        ViewBag.MsjPantalla = "No se puede agregar. Ya existe una reservacion para esa fecha";
                        return View("AddReservation");
                    }
                }
                else
                {
                    // Si hay errores de validación, vuelve a la vista de registro con los mensajes de error
                    return View("AddReservation", entidad);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en Registrar Reserva: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}