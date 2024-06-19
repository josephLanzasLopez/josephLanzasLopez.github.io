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
    public class DishController : Controller
    {
        DishModel model = new DishModel();




        private bool ObtenerParaLlevar()
        {
            // Puedes obtener este valor desde el ViewData
            bool paraLlevar = ViewData["ParaLlevar"] as bool? ?? false;
            return paraLlevar;
        }


        // Ejemplo de cómo podrías obtener el valor de "nota" desde la vista
        private string ObtenerNota()
        {
            // Puedes obtener este valor desde el ViewData
            string nota = ViewData["Nota"] as string ?? string.Empty;
            return nota;
        }


        public async Task<ActionResult> About()
        {
            //id de la autorizacion para obtener el dinero
            string token = Request.QueryString["token"];

            bool status = false;

            using (var client = new HttpClient())
            {
                var userName = "Ac4o9Ls_j3QbJCO1jl8rGDMFDZ6V0mAEG7exPjxYIMcEoFSlS9qqCzW877gRxKLRiakmGdvFMOkCOzsI";
                var passwd = "EJMvVLT303jt8i_Paf9KqkNbVy72gKLqlho3qVj2F27DUJ8R8iCrMA7xm_ZQdEeNbwIYjhqJDmvunhD8";

                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var data = new StringContent("{}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);

                status = response.IsSuccessStatusCode;

                ViewData["Status"] = status;
                if (status)
                {
                    var jsonRespuesta = response.Content.ReadAsStringAsync().Result;
                    PaypalTransaction objeto = JsonConvert.DeserializeObject<PaypalTransaction>(jsonRespuesta);
                    ViewData["IdTransaccion"] = objeto.purchase_units[0].payments.captures[0].id;

                    bool paraLlevar = ObtenerParaLlevar();
                    string nota = ObtenerNota();

                    // Establecer valores en ViewData
                    ViewData["ParaLlevar"] = paraLlevar;
                    ViewData["Nota"] = nota;
                    CrearFactura(paraLlevar, nota);

                    RemoveAllFromCart();
                }
            }

            return View();
        }

        // Ajusta la firma del método CrearFactura para incluir los nuevos parámetros
        private void CrearFactura(bool paraLlevar, string nota)
        {
            try
            {
                long userId = long.Parse(Session["IdUsuario"].ToString());
                using (var client = new HttpClient())
                {
                    string token = ControllerContext.HttpContext.Session["Token"].ToString();

                    // Modificar la URL para reflejar los cambios en la API
                    string url = ConfigurationManager.AppSettings["urlApi"].ToString() + $"api/Factura?userId={userId}&paraLlevar={paraLlevar}&nota={nota}";

                    // No es necesario enviar paypalTransactionId si no se necesita
                    var data = new { };

                    // Convertir el objeto a formato JSON
                    string jsonData = JsonConvert.SerializeObject(data);

                    // Configurar la solicitud POST con el cuerpo JSON
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Modificar la solicitud DELETE a solicitud POST
                    HttpResponseMessage resp = client.PostAsync(url, content).Result;

                    if (resp.IsSuccessStatusCode)
                    {
                        // Redirigir o realizar otras acciones después del éxito
                        RedirectToAction("Menu", "Dish");
                    }

                    // Manejar el caso en que la solicitud no sea exitosa
                    ViewBag.ErrorMessage = "Error al procesar la factura. Por favor, inténtelo de nuevo.";
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones, loggear, etc.
                ViewBag.ErrorMessage = "Error al procesar la factura. Por favor, inténtelo de nuevo.";
            }
        }




       




        [HttpPost]
        public ActionResult UpdateDishCartQuantity(long dishCartId, int quantity)
        {
            try
            {
                // Crear un objeto UpdateQuantityRequest con la información necesaria
                var updateRequest = new UpdateQuantityRequest
                {
                    DishCartId = dishCartId,
                    Quantity = quantity
                };

                // Llamar al método correspondiente en el modelo
                var result = model.UpdateDishCartQuantity(updateRequest);

                if (result > 0)
                {
                    // Éxito: Puedes redirigir a una vista o realizar otras acciones necesarias.
                    return RedirectToAction("Menu", "Dish");
                }
                else
                {
                    // Manejar el caso en que la actualización no sea exitosa
                    ViewBag.ErrorMessage = "Error al actualizar la cantidad del plato en el carrito. Por favor, inténtelo de nuevo.";
                    return View("NombreDeTuVista");
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones, loggear, etc.
                ViewBag.ErrorMessage = "Error al actualizar la cantidad del plato en el carrito. Por favor, inténtelo de nuevo.";
                return View("NombreDeTuVista");
            }
        }








        [HttpPost]
        public ActionResult RemoveAllFromCart()
        {
            long userId = long.Parse(Session["IdUsuario"].ToString()); // Obtener el IdUsuario de la sesión

            using (var client = new HttpClient())
            {
                string token = ControllerContext.HttpContext.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/RemoveAllDishCart?userId=" + userId;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.DeleteAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    // Redirigir o realizar otras acciones después de la eliminación exitosa
                    return RedirectToAction("Menu", "Dish");
                }

                // Manejar el caso en que la eliminación no sea exitosa
                ViewBag.ErrorMessage = "Error al eliminar elementos del carrito. Por favor, inténtelo de nuevo.";
                return View("NombreDeTuVista");
            }
        }

        [HttpGet]
        public ActionResult Menu()
        {
            UpdateTotals();

            var resp = model.CheckDishes();
            return View(resp);
        }

        [HttpGet]
        public ActionResult AddDishCart(long q)
        { 
            CartEnt entidad = new CartEnt();
            entidad.IdDish = q;
            entidad.IdUsuario = long.Parse(Session["IdUsuario"].ToString());
            entidad.RegistDate = DateTime.Now;

            var resp = model.AddDishCart(entidad);
            UpdateTotals();

            if (resp > 0)
                return RedirectToAction("Menu", "Dish");
            else
            {
                ViewBag.MsjPantalla = "El plato ya ha sido comprado o añadido a tu carrito de compras";
                var dishes = model.CheckDishes();
                return View("../Dish/Menu", dishes);
            }
        }


    


        [HttpPost]
        public ActionResult RemoveDish(long q)
        {
            var resp = model.RemoveDish(q);
            UpdateTotalsDish();

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        private void UpdateTotalsDish()
        {
            var CurrentDish = model.CheckDish(long.Parse(Session["IdUsuario"].ToString()));
        
        }




        [HttpPost]
        public ActionResult RemoveDishCart(long q)
        {
            var resp = model.RemoveDishCart(q);
            UpdateTotals();

            return Json("OK", JsonRequestBehavior.AllowGet);
        }



        private void UpdateTotals()
        {
            var CurrentCart = model.CheckCart(long.Parse(Session["IdUsuario"].ToString()));
            Session["Amount"] = CurrentCart.Count();
            Session["SubTotal"] = CurrentCart.Sum(x => x.Price);
            Session["Total"] = CurrentCart.Sum(x => x.Price) + (CurrentCart.Sum(x => x.Price) * 0.13M);
        }

        [HttpGet]
        public ActionResult CheckPayment()
        {
            var date = model.CheckCart(long.Parse(Session["IdUsuario"].ToString()));
            return View(date); 
        }

        [HttpPost]
        public ActionResult ConfirmPaymentCart()
        {
            CartEnt entidad = new CartEnt();
            entidad.IdUsuario = long.Parse(Session["IdUsuario"].ToString());

            model.ConfirmPaymentCart(entidad);
            return RedirectToAction("Menu", "Dish");
        }

        [HttpGet]
        public ActionResult CheckMyDishes()
        {
            var datos = model.CheckMyDishes(long.Parse(Session["IdUsuario"].ToString()));
            return View(datos);
        }


        [HttpGet]
        public ActionResult CheckCartF()
        {
            var datos = model.CheckFactura(long.Parse(Session["IdUsuario"].ToString()));
            return View(datos);
        }

        [HttpGet]
        public ActionResult CheckDishes()
        {
            var datos = model.CheckDishes();
            return View(datos);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddDish(HttpPostedFileBase ImageDish, DishEnt entidad)
        {
            //Registro de plato
            entidad.Image = string.Empty;
            var IdDish = model.RegisterDish(entidad);
            entidad.IdDish = IdDish;

            //Guardar la imagen
            string extension = Path.GetExtension(Path.GetFileName(ImageDish.FileName));
            //string ruta = ConfigurationManager.AppSettings["pathSaveImages"] + IdDish + extension;
            //ImageDish.SaveAs(ruta);

            string carpetaImagenes = "~/images/";  // Ruta relativa dentro de tu aplicación
            string nombreArchivo = IdDish.ToString();  // Nombre del archivo basado en algún identificador único
            string ruta = Path.Combine(Server.MapPath(carpetaImagenes), IdDish + extension);

            ImageDish.SaveAs(ruta);

            
            entidad.Image = ConfigurationManager.AppSettings["pathSaveDataBase"] + IdDish + extension;
            model.UpdatePathDish(entidad);

            return RedirectToAction("CheckDishes", "Dish");
        }



        [HttpGet]
        public ActionResult Edit(long q)
        {
            var datos = model.CheckDish(q);
            return View(datos);
        }



        [HttpPost]
        public ActionResult UpdateDish(HttpPostedFileBase ImageDish, DishEnt entidad)
        {
            var IdDish = entidad.IdDish;

            if (ImageDish != null && ImageDish.ContentLength > 0) // Si se proporciona una nueva imagen
            {
                // Eliminar la imagen anterior
                string extension = Path.GetExtension(Path.GetFileName(ImageDish.FileName));
                string carpetaImagenes = "~/images/";
                string rutaAntigua = Path.Combine(Server.MapPath(carpetaImagenes), IdDish + extension);

                if (System.IO.File.Exists(rutaAntigua))
                {
                    System.IO.File.Delete(rutaAntigua);
                }

                // Guardar la nueva imagen
                string nuevaRuta = Path.Combine(Server.MapPath(carpetaImagenes), IdDish + extension);
                ImageDish.SaveAs(nuevaRuta);

                // Actualizar el campo de imagen en la entidad
                entidad.Image = ConfigurationManager.AppSettings["pathSaveDataBase"] + IdDish + extension;
            }

            // Actualizar otros datos del plato (excepto la imagen)
            model.UpdateDish(entidad);

            return RedirectToAction("CheckDishes", "Dish");
        }
    }
}