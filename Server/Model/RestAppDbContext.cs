using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using MySql.Data.Entity;

namespace Server.Model
{
    [DbConfigurationType(typeof (MySqlEFConfiguration))]
    public class RestAppDbContext : DbContext
    {

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Place> Places { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }

        public RestAppDbContext()
            : base("name=RestAppDbContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<RestAppDbContext>());
            Database.Log += m => Debug.WriteLine(m);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties().Where(p => p.Name == "RowVersion").Configure(p =>
                p.IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None));
        }

        public override int SaveChanges()
        {
            var concurrencyTokenEntries =
                ChangeTracker.Entries<IVersioned>().Where(e => e.State != EntityState.Unchanged);
            foreach (var entry in concurrencyTokenEntries)
            {
                entry.Entity.RowVersion = Guid.NewGuid();
            }
            return base.SaveChanges();
        }
    }
}