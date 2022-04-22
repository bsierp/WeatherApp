using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace OpenWeatherMapApp
{
    /// <summary>
    /// Klasa zawierająca informacje o koordynatach uzyskanych z JSON
    /// </summary>
    public class Cooridnates
    {
        /// <summary>
        /// Szerokość geograficzna
        /// </summary>
        public float lon { get; set; }
        /// <summary>
        /// Długość geograficzna
        /// </summary>
        public float lat { get; set; }
    }
    /// <summary>
    /// Klasa zawierająca informacje o pogodzie uzyskanej z JSON
    /// </summary>
    public class Weather
    {
        /// <summary>
        /// Ogólny opis pogody
        /// </summary>
        public string main { get; set; }
        /// <summary>
        /// Szczegóły dotyczące pogody
        /// </summary>
        public string description { get; set; }
    }
    /// <summary>
    ///  Klasa zawierająca informacje o głównych informacjach pogodowych uzyskanych z JSON
    /// </summary>
    public class Main_info
    {
        /// <summary>
        /// Temperatura rzeczywista w °C
        /// </summary>
        public float temp { get; set; }
        /// <summary>
        /// Temperatura odczuwalna °C
        /// </summary>
        public float feels_like { get; set; }
        /// <summary>
        /// Ciśnienie atmosferyczne w hPa
        /// </summary>
        public float pressure { get; set; }
        /// <summary>
        /// Wilgotność w procentach
        /// </summary>
        public float humidity { get; set; }
    }
    /// <summary>
    /// Klasa zawierająca informacje o prędkości wiatru uzyskanej z JSON
    /// </summary>
    public class Wind
    {
        //Prędkość wiatru w m/s
        public float speed { get; set; }
    }
    /// <summary>
    /// Klasa zawierająca informacje o wschodzie i zachodzie słońca
    /// </summary>
    public class Sun_Info
    {
        /// <summary>
        /// Wschód słońca w formie UnixTimeStamp
        /// </summary>
        public long sunrise { get; set; }
        /// <summary>
        /// Zachód słońca w formie UnixTimeStamp
        /// </summary>
        public long sunset { get; set; }
    }
    /// <summary>
    /// Klasa służąca od deserializowania formatu JSON
    /// </summary>
    public class WeatherData
    {
        /// <summary>
        /// Id uzyskane z JSON
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nazwa miasta uzyskana z JSON
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Koordynaty
        /// </summary>
        public Cooridnates coord { get; set; }
        /// <summary>
        /// Opis pogody
        /// </summary>
        public Weather[] weather { get; set; }
        /// <summary>
        /// Główne dane
        /// </summary>
        public Main_info main { get; set; }
        /// <summary>
        /// Prędkość wiatru
        /// </summary>
        public Wind wind { get; set; }
        /// <summary>
        /// Wschód/Zachód słońca
        /// </summary>
        public Sun_Info sys { get; set; }
        /// <summary>
        /// Metoda zwracająca odpowiedź JSON z api jako string
        /// </summary>
        /// <param name="city_name">Nazwa miasta do odczytania informacji.</param>
        /// <returns>JSON w formacie string.</returns>
        public static string download_weather(string city_name)
        {
            using (WebClient web = new WebClient())
            {
                    string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q=" + city_name + "&appid=8eb9096c261c3bc7377c87264110c1b0&units=metric");
                    var json = web.DownloadString(url);
                    return json;
            }
        }
        /// <summary>
        /// Metoda konwertująca JSON na klasę WeatherData
        /// </summary>
        /// <param name="city_name">Nazwa miasta do odczytania informacji.</param>
        /// <returns>JSON przekonwertowany na obiekt WeatherData</returns>
        public static WeatherData download_weather_to_weather_data_object(string city_name)
        {

                using (WebClient web = new WebClient())
                {
                    return JsonConvert.DeserializeObject<WeatherData>(download_weather(city_name));
                }
        }
    }
}
