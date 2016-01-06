using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model
{
    public class Restaurant : IVersioned
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public User Owner { get; set; }

        public ICollection<Place> Places { get; set; }
        
        public Guid RowVersion { get; set; }
    }
}