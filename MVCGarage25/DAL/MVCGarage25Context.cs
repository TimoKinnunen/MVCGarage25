using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MVCGarage25.DAL
{
    public class MVCGarage25Context : DbContext
    {
        public MVCGarage25Context() : base("MVCGarage25")
        {


        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<Models.Member> Members { get; set; }
        public DbSet<Models.Vehicle> Vehicles { get; set; }
        public DbSet<Models.VehicleType> VehicleTypes { get; set; }
    }
}