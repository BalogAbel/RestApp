using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model
{
    public class User : IVersioned
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        public ICollection<Restaurant> Restaurants { get; set; }

        public Guid RowVersion { get; set; }
    }
}