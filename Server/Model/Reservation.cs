using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model
{
    public class Reservation : IVersioned
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Place Place { get; set; }

        public ICollection<Seat> Seats { get; set; }

        public User Guest { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public Guid RowVersion { get; set; }
    }
}