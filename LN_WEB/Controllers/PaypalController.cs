using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Web.Mvc;
using LN_WEB.Models.Paypal_Transaction;
using LN_WEB.Models.Paypal_Order;
using LN_WEB.Entities;
using System.Configuration;
using System.Web.WebPages;

namespace LN_WEB.Controllers
{
    public class PaypalController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        
        

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Paypal(decimal total)
        {
            bool status = false;
            string respuesta = string.Empty;

            using (var client = new HttpClient())
            {

                var userName = "AWzWfST4G0J6kpS1rLYr-UkhazKkbf1Y92QJk_DCFLdcXWeNgVHgFlJ-6SrLDzqpok_Kiu8D-YsX8dZy";
                var passwd = "ECjPI_QS9ML5Vk5qVOVQldhsyP7hdnhRR8mB9htRHp_17WBKX_ZMxCjCdLfnNZT5_sGUAgTammfBo4P1";

                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                // Crear la unidad de compra para PayPal
                var purchaseUnit = new Models.Paypal_Order.PurchaseUnit()
                {
                    amount = new Models.Paypal_Order.Amount()
                    {
                        currency_code = "USD",
                        value = total.ToString()
                    },
                    description = "Total del carrito" //
                };


                var orden = new PaypalOrder()
                {
                    intent = "CAPTURE",
                    purchase_units = new List<Models.Paypal_Order.PurchaseUnit> { purchaseUnit },
                    application_context = new ApplicationContext()
                    {
                        brand_name = "Mi Tienda",
                        landing_page = "NO_PREFERENCE",
                        user_action = "PAY_NOW",
                        return_url = "https://appwebvaron.azurewebsites.net/Dish/About",
                        cancel_url = "https://appwebvaron.azurewebsites.net/Dish/CheckPayment"
                    }
                };

                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);

                status = response.IsSuccessStatusCode;

                if (status)
                {
                    respuesta = response.Content.ReadAsStringAsync().Result;

                }

            }

            return Json(new { status = status, respuesta = respuesta }, JsonRequestBehavior.AllowGet);
        }







    }

}
