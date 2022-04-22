using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMapApp
{
    /// <summary>
    /// Klasa modelująca obiekt ulubionego miasta w bazie danych
    /// </summary>
    public class FavouriteCity
    {
        /// <summary>
        /// ID w bazie danych
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Nazwa miasta
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Dane uzyskane z api w formie JSON'a
        /// </summary>
        public string WeatherData { get; set; }
        /// <summary>
        /// Data uzyskania danych
        /// </summary>
        public DateTime date { get; set; }
        /// <summary>
        /// Zmienna używana do ustawiania miasta na ekran początkowy aplikacji
        /// </summary>
        public bool onStart { get; set; }        
    }
}
