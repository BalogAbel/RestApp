using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public List<Reservation> Reservations { get; set; }

        public List<Restaurant> Restaurants { get; set; }
    }
}