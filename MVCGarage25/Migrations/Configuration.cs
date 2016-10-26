namespace MVCGarage25.Migrations
{
    using Models;
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCGarage25.DAL.MVCGarage25Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MVCGarage25.DAL.MVCGarage25Context context)
        {
            // context.VehicleTypes.AddOrUpdate(
            //  p => p.Type,
            //  new VehicleType { Type = "Passenger car" },
            //  new VehicleType { Type = "Truck" },
            //  new VehicleType { Type = "Bus" }
            //);

            var vehicleTypes = new VehicleType[]
            {
                new VehicleType { Type = "Passenger car" },
                new VehicleType { Type = "Truck" },
                new VehicleType { Type = "Dubber bubber" },
                new VehicleType { Type = "Bus" }
            };

            context.VehicleTypes.AddOrUpdate(v => v.Type, vehicleTypes);
            context.SaveChanges();

            var members = new Member[]
            {
             new Member { FirstName = "Prins", LastName = "Bertil" },
             new Member { FirstName = "Knatte", LastName = "Anka" },
             new Member { FirstName = "Rolf", LastName = "Persson" },
             new Member { FirstName = "Fnatte", LastName = "Anka" }
            };

            context.Members.AddOrUpdate(m => new { m.FirstName, m.LastName }, members);
            context.SaveChanges();

            var vehicles = new Vehicle[]
            {
                new Vehicle {MemberId=members[0].Id, RegistrationNumber = "UYB512", StartParkingTime=DateTime.Now, NumberOfWheels = 4, BrandAndModel = "Peugeot 206RC", Color = "Silver metallic", VehicleTypeId = vehicleTypes[0].Id },
                new Vehicle {MemberId=members[1].Id, RegistrationNumber = "YYB513",StartParkingTime=DateTime.Now,  NumberOfWheels = 4, BrandAndModel = "Peugeot 206RC", Color = "Silver metallic", VehicleTypeId = vehicleTypes[1].Id },
                new Vehicle {MemberId=members[2].Id, RegistrationNumber = "XYB514", StartParkingTime=DateTime.Now, NumberOfWheels = 4, BrandAndModel = "Peugeot 206RC", Color = "Silver metallic", VehicleTypeId = vehicleTypes[2].Id }
            };

            context.Vehicles.AddOrUpdate(v => v.RegistrationNumber, vehicles);
            context.SaveChanges();

        }
    }
}
