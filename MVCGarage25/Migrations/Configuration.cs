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

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

        /*
 protected override void Seed(DAL.MVCGarageDbContext context)
        {
            context.VehicleTypes.AddOrUpdate(
              p => p.Type,
              new VehicleType { Type = "Passenger car" },
              new VehicleType { Type = "Truck" },
              new VehicleType { Type = "Bus" }
            );


            context.SaveChanges();

            var vehicleTypes = context.VehicleTypes.ToList();

            context.Vehicles.AddOrUpdate(
              p => p.RegistrationNumber,
              new Vehicle
              {
                  RegistrationNumber = "UYB123",
                  StartParkingTime = DateTime.Now.AddHours(-1),
                  ParkingCostPerHour = 60,
                  VehicleTypeId = vehicleTypes[0].Id
              },
              new Vehicle
              {
                  RegistrationNumber = "XYB123",
                  StartParkingTime = DateTime.Now.AddHours(-1),
                  ParkingCostPerHour = 60,
                  VehicleTypeId = vehicleTypes[0].Id
              },
              new Vehicle
              {
                  RegistrationNumber = "AYB123",
                  StartParkingTime = DateTime.Now.AddHours(-1),
                  ParkingCostPerHour = 60,
                  VehicleTypeId = vehicleTypes[0].Id
              }
            );
        }        */
    }
}
