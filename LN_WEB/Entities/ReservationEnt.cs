using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LN_WEB.Entities
{
    public class ReservationEnt
    {
        public long IdReservation { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }
        public DateTime DateReservation { get; set; }
        public int NumberPeople { get; set; }
        public long TableID { get; set; }
        public bool State { get; set; }
    }
}