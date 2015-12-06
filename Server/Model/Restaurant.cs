using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model
{
    public class Restaurant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public User Owner { get; set; }

        public IEnumerable<Place> Places { get; set; } 
    }
}