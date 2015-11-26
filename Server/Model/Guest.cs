using System.Collections.Generic;

namespace Server.Model
{
    public class Guest : User
    {
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}