namespace MVCGarage25.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.FirstName, t.LastName }, unique: true, name: "IX_MemberFullName");
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        VehicleTypeId = c.Int(nullable: false),
                        RegistrationNumber = c.String(nullable: false, maxLength: 100),
                        StartParkingTime = c.DateTime(nullable: false),
                        EndParkingTime = c.DateTime(),
                        NumberOfWheels = c.Int(),
                        BrandAndModel = c.String(),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Members", t => t.MemberId)
                .ForeignKey("dbo.VehicleTypes", t => t.VehicleTypeId)
                .Index(t => t.MemberId)
                .Index(t => t.VehicleTypeId)
                .Index(t => new { t.RegistrationNumber, t.EndParkingTime }, unique: true, name: "IX_UniqueParkedRegNo");
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Type, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "VehicleTypeId", "dbo.VehicleTypes");
            DropForeignKey("dbo.Vehicles", "MemberId", "dbo.Members");
            DropIndex("dbo.VehicleTypes", new[] { "Type" });
            DropIndex("dbo.Vehicles", "IX_UniqueParkedRegNo");
            DropIndex("dbo.Vehicles", new[] { "VehicleTypeId" });
            DropIndex("dbo.Vehicles", new[] { "MemberId" });
            DropIndex("dbo.Members", "IX_MemberFullName");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Members");
        }
    }
}
