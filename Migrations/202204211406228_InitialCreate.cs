namespace OpenWeatherMapApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WeatherDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        coord_lon = c.Single(nullable: false),
                        coord_lat = c.Single(nullable: false),
                        main_temp = c.Single(nullable: false),
                        main_feels_like = c.Single(nullable: false),
                        main_pressure = c.Single(nullable: false),
                        main_humidity = c.Single(nullable: false),
                        wind_speed = c.Single(nullable: false),
                        sys_sunrise = c.Long(nullable: false),
                        sys_sunset = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.FavouriteCities", "date", c => c.DateTime(nullable: false));
            AddColumn("dbo.FavouriteCities", "WeatherData_Id", c => c.Int());
            CreateIndex("dbo.FavouriteCities", "WeatherData_Id");
            AddForeignKey("dbo.FavouriteCities", "WeatherData_Id", "dbo.WeatherDatas", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FavouriteCities", "WeatherData_Id", "dbo.WeatherDatas");
            DropIndex("dbo.FavouriteCities", new[] { "WeatherData_Id" });
            DropColumn("dbo.FavouriteCities", "WeatherData_Id");
            DropColumn("dbo.FavouriteCities", "date");
            DropTable("dbo.WeatherDatas");
        }
    }
}
