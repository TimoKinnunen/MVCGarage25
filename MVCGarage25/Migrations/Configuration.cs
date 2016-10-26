namespace MVCGarage25.Migrations
{
    using Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCGarage25.DAL.MVCGarage25Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCGarage25.DAL.MVCGarage25Context context)
        {
            context.VehicleTypes.AddOrUpdate(
             p => p.Type,
             new VehicleType { Type = "Passenger car" },
             new VehicleType { Type = "Truck" },
             new VehicleType { Type = "Bus" }
           );

            context.Members.AddOrUpdate(
             m => new { m.FirstName, m.LastName },
             new Member { FirstName = "Prins", LastName = "Bertil" },
             new Member { FirstName = "Knatte", LastName = "Anka" },
             new Member { FirstName = "Fnatte", LastName = "Anka" }
           );

        }
    }
}
