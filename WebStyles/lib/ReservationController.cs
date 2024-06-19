using LN_API.Entities;
using LN_API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace LN_API.Controllers
{
    [Authorize]
    public class ReservationsController : ApiController
    {

        //[HttpGet]
        //[Route("api/CheckReservations")]
        //public List<ReservationEnt> CheckReservations()
        //{
        //    using (var bd = new EL_VARONEntities())
        //    {
        //        var datos = (from x in bd.Reservation
        //                     select x).ToList();

        //        if (datos.Count > 0)
        //        {
        //            var resp = new List<ReservationEnt>();
        //            foreach (var item in datos)
        //            {
        //                resp.Add(new ReservationEnt
        //                {
        //                    IdReservation = item.IdReservation,
        //                    Name = item.Name,
        //                    Phone = item.Phone,
        //                    DateReservation = item.DateReservation,
        //                    NumberPeople = item.NumberPeople,
        //                    TableID = item.TableID,
        //                    State = item.State,
        //                });
        //            }

        //            return resp;
        //        }
        //        else
        //        {
        //            return new List<ReservationEnt>();
        //        }
        //    }
        //}

        [HttpGet]
        [Route("api/ReservasAgendadas")]
        public List<Reservados> ReservasAgendadas(string fechaInicio, string fechaFin, string idReserva)
        {
            var bd = new EL_VARONEntities();
            List<Reservados> lista = new List<Reservados>();

            try
            {
                using (SqlConnection connection = (SqlConnection)bd.Database.Connection)
                {
                    SqlCommand cmd = new SqlCommand("sp_ObtenerReservasAgendadas", connection);
                    cmd.Parameters.AddWithValue("@fechainicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@fechafin", fechaFin);
                    cmd.Parameters.AddWithValue("@idReserva", string.IsNullOrEmpty(idReserva) ? " " : idReserva);
                    cmd.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Reservados()
                            {
                                FechaVenta = dr["FechaVenta"].ToString(),
                                Reserva = dr["Reserva"].ToString(),
                                Total = Convert.ToInt32(dr["Total"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error: {ex.Message}");
                lista = new List<Reservados>();
            }
            finally
            {
                // Asegurarse de cerrar la conexión en todos los casos
                bd.Database.Connection.Close();
            }

            return lista;
        }




        [HttpGet]
        [Route("api/CheckReservation")]
        public ReservationEnt CheckReservation(long q)
        {
            using (var bd = new EL_VARONEntities())
            {
                var datos = (from x in bd.Reservation
                             where x.IdReservation == q
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    ReservationEnt resp = new ReservationEnt
                    {
                        IdReservation = datos.IdReservation,
                        Name = datos.Name,
                        Phone = datos.Phone,
                        TableID = datos.TableID,
                        DateReservation = datos.DateReservation,
                        NumberPeople = datos.NumberPeople,
                        State = datos.State
                    };

                    return resp;
                }
                else
                {
                    return null;
                }
            }
        }



        [HttpDelete]
        [Route("api/RemoveReservation")]
        public int RemoveReservation(long q)
        {
            using (var bd = new EL_VARONEntities())
            {
                var Reservation = (from x in bd.Reservation
                                   where x.IdReservation == q
                                   select x).FirstOrDefault();

                if (Reservation != null)
                {
                    bd.Reservation.Remove(Reservation);
                    return bd.SaveChanges();
                }

                return 0;
            }
        }


        private long GetNextReservationId()
        {
            // Implementa lógica para generar el siguiente ID disponible, por ejemplo, puedes consultar la base de datos para obtener el último ID y sumarle 1
            // Aquí hay un ejemplo básico para demostración, asegúrate de implementar una lógica adecuada para tu aplicación
            using (var bd = new EL_VARONEntities())
            {
                long lastId = bd.Reservation.Max(r => (long?)r.IdReservation) ?? 0;
                return lastId + 1;
            }
        }


        [HttpPost]
        [Route("api/RegisterReservation")]
        [AllowAnonymous]
        public long RegisterReservation(ReservationEnt entidad)
        {
          
                using (var bd = new EL_VARONEntities())
                {
                Reservation tabla = new Reservation();

                tabla.Name = entidad.Name;
                tabla.Phone = entidad.Phone;
                tabla.DateReservation = entidad.DateReservation;
                tabla.NumberPeople = entidad.NumberPeople;
                tabla.TableID = entidad.TableID;
                tabla.State = entidad.State;

                bd.Reservation.Add(tabla);
                bd.SaveChanges();


                return tabla.IdReservation; // Returns the result as HTTP 200 OK response
                }
            
           
        }


        [HttpPut]
        [Route("api/UpdateReservation")]
        public IHttpActionResult UpdateReservation(ReservationEnt entidad)
        {
            try
            {
                using (var bd = new EL_VARONEntities())
                {
                    var reservation = (from x in bd.Reservation
                                       where x.IdReservation == entidad.IdReservation
                                       select x).FirstOrDefault();

                    if (reservation != null)
                    {
                        reservation.Name = entidad.Name; // Include Name field
                        reservation.Phone = entidad.Phone; // Include Phone field
                        reservation.DateReservation = entidad.DateReservation;
                        reservation.NumberPeople = entidad.NumberPeople;
                        reservation.TableID = entidad.TableID;
                        reservation.State = entidad.State;

                        bd.SaveChanges();

                        return Ok(); // Returns HTTP 200 OK
                    }
                    else
                    {
                        return NotFound(); // Returns HTTP 404 Not Found if the reservation is not found
                    }
                }
            }
            catch (Exception ex)
            {
                // Logs and handles the exception
                Console.WriteLine("Error in Update Reservation: " + ex.Message, ex);
                return InternalServerError(); // Returns HTTP 500 Internal Server Error
            }
        }





        [HttpGet]
        [Route("api/CheckMyReservations")]
        public List<ReservationEnt> CheckMyReservations(long q)
        {
            using (var bd = new EL_VARONEntities())
            {
                var datos = (from x in bd.ReservationUser
                             join y in bd.Reservation on x.IdReservation equals y.IdReservation
                             where x.IdReservation == q
                             select new
                             {
                                 x.IdReservationUser,
                                 x.IdReservation,
                                 x.IdUsuario,
                                 x.DateReservation,

                                }).ToList();

                if (datos.Count > 0)
                {
                    var resp = new List<ReservationEnt>();
                    foreach (var item in datos)
                    {
                        resp.Add(new ReservationEnt
                        {
                            IdReservation = item.IdReservation,
                            DateReservation = item.DateReservation

                        });
                    }

                    return resp;
                }
                else
                {
                    return new List<ReservationEnt>();
                }
            }
        }

        [HttpGet]
        [Route("api/CheckReservationsDates")]
        public IHttpActionResult CheckReservationsDates()
        {
            try
            {
                using (var context = new EL_VARONEntities())
                {
                    // Obtener las fechas de las reservaciones
                    var reservations = context.Reservation.Select(r => new ReservationEnt { DateReservation = r.DateReservation }).ToList();

                    return Ok(reservations);
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y devolver un error HTTP 500
                return InternalServerError(ex);
            }
        }







        [HttpGet]
        [Route("api/ObtenerReservaciones")]
        public IHttpActionResult GetReservations()
        {
            try
            {
                using (var context = new EL_VARONEntities())
                {
                    // Obtener las reservaciones de la base de datos
                    var reservations = context.Reservation.Select(r => new
                    {
                        title = "Reservacion",
                        start = $"{r.DateReservation:yyyy-MM-dd}", // Ajustar según la lógica de tu aplicación
                        color = "blue"
                    }).ToList();

                    return Ok(reservations);
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y devolver un error HTTP 500
                return InternalServerError(ex);
            }
        }


    }
}

