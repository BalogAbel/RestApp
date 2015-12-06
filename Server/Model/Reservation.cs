using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model
{
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Place Place { get; set; }

        public IEnumerable<Tuple<int, int>> Seats { get; set; }

        public User Guest { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}