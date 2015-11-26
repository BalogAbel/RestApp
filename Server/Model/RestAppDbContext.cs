using System.Data.Entity;
using System.Diagnostics;
using MySql.Data.Entity;

namespace Server.Model
{
    [DbConfigurationType(typeof (MySqlEFConfiguration))]
    public class RestAppDbContext : DbContext
    {
        // Your context has been configured to use a 'RestAppDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Server.RestAppDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'RestAppDbContext' 
        // connection string in the application configuration file.
        public RestAppDbContext()
            : base("name=RestAppDbContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<RestAppDbContext>());
            Database.Log += m => Debug.WriteLine(m);
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
    }
}