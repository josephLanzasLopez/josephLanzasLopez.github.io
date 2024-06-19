
using LN_WEB.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace LN_WEB.Models
{
    public class DashBoardModel
    {


        public List<Reporte> ObtenerVentas(string fechainicio, string fechafin, string idFactura)
        {
            using (var client = new HttpClient())
            {
                string token = HttpContext.Current.Session["Token"].ToString();
                string url = ConfigurationManager.AppSettings["urlApi"].ToString() + $"api/Ventas?fechainicio={fechainicio}&fechafin={fechafin}&idFactura={idFactura}";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage resp = client.GetAsync(url).Result;

                if (resp.IsSuccessStatusCode)
                {
                    return resp.Content.ReadFromJsonAsync<List<Reporte>>().Result;
                }

                return new List<Reporte>();
            }
        }


            public DashBoardEnt ObtenerReporteDashboard()
        {
            try
            {
                // Llamar al servicio de la API para obtener los datos del informe del dashboard
                using (var client = new HttpClient())
                {
                    string urlApi = ConfigurationManager.AppSettings["urlApi"].ToString() + "api/ObtenerReporteDashboard";
                    string token = HttpContext.Current.Session["Token"].ToString();

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage resp = client.GetAsync(urlApi).Result;

                    if (resp.IsSuccessStatusCode)
                    {
                        // Deserializar la respuesta y devolver los datos del informe del dashboard
                        return resp.Content.ReadFromJsonAsync<DashBoardEnt>().Result;
                    }
                    else
                    {
                        // Manejar el caso en el que la solicitud no fue exitosa
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }


    }
}