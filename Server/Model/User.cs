using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public Guid Token { get; set; }
    }
}