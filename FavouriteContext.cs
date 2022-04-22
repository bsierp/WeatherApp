using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace OpenWeatherMapApp
{
    public class FavouriteContext : DbContext
    {
        // Your context has been configured to use a 'FavouriteContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'OpenWeatherMapApp.FavouriteContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'FavouriteContext' 
        // connection string in the application configuration file.
        public FavouriteContext()
            : base("name=FavouriteContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<FavouriteCity> city_info { get; set; }



        public class FavouriteCitiesDbInitializer : DropCreateDatabaseAlways<FavouriteContext>
        {
            protected override void Seed(FavouriteContext context)
            {
                var cities = new List<FavouriteCity>
                {
                    new FavouriteCity()
                    {
                        ID = 1,
                        Name = "Wroc³aw",
                        WeatherData = WeatherData.download_weather("Wroc³aw"),
                        date = DateTime.Now
                    },
                    new FavouriteCity()
                    {
                        ID = 2,
                        Name = "Warszawa",
                        WeatherData = WeatherData.download_weather("Warszawa"),
                        date = DateTime.Now
                    }
                };
                cities.ForEach(s => context.city_info.Add(s));
                context.SaveChanges();
            }
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}