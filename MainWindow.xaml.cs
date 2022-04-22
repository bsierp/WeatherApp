using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
namespace OpenWeatherMapApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Funkcja konwertująca UnixTimeStamp na DateTime
        /// </summary>
        /// <param name="unixTimeStamp">Czas w formacie UnixTimeStamp</param>
        /// <returns>Czas w formacie DateTime</returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        string current_city; // Miasto aktualnie wyświetlane
        public MainWindow()
        {
            InitializeComponent();
            IList<FavouriteCity> city;
            FavouriteCity showOnStart = null;
            using (var context = new FavouriteContext())
            {
                //Na start aplikacji aktualizujemy dane dla ulubionych miast
                foreach (FavouriteCity city_entry in context.city_info)
                {
                    context.Entry(city_entry).Entity.WeatherData = WeatherData.download_weather(city_entry.Name);
                    context.Entry(city_entry).Entity.date = DateTime.Now;
                }
                context.SaveChanges();
                city = context.city_info.ToList();
                showOnStart = context.city_info.Where(x => x.onStart == true).SingleOrDefault<FavouriteCity>();
            }
            FavouriteCities.ItemsSource = city;
            if (showOnStart != null)
            {
                try
                {
                    WeatherData deserializedData = WeatherData.download_weather_to_weather_data_object(showOnStart.Name);
                    current_city = showOnStart.Name;
                    string data = "City: " + showOnStart.Name + "\n";
                    data += "Coords: " + deserializedData.coord.lat + ", " + deserializedData.coord.lon + "\n";
                    data += "Weather: " + deserializedData.weather[0].main + ", " + deserializedData.weather[0].description + "\n";
                    data += "Temperature: " + deserializedData.main.temp + "°C, " + "Feels Like: " + deserializedData.main.feels_like + "°C\n";
                    data += "Pressure: " + deserializedData.main.pressure + "hPa\n" + "Humidity: " + deserializedData.main.humidity + "%\n";
                    data += "Wind speed: " + deserializedData.wind.speed + "m/s\n";
                    data += "Sunrise: " + UnixTimeStampToDateTime(deserializedData.sys.sunrise) + "\n";
                    data += "Sunset: " + UnixTimeStampToDateTime(deserializedData.sys.sunset) + "\n";
                    Test_Json_Data.Text = data;
                } catch (Exception ex)
                {
                    MessageBox.Show("Connection error.\nErrorLog:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }
        /// <summary>
        /// Odpowiedź na kliknięcie przycisku pobierającego pogodę, wypisuje pogodę z miasta wpisanego do formularza.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Get_Weather(object sender, RoutedEventArgs e)
        {
            try
            {
                string city = Input_Box.Text;
                WeatherData deserializedData = WeatherData.download_weather_to_weather_data_object(city);
                current_city = city;
                string data = "City: " + city + "\n";
                data += "Coords: " + deserializedData.coord.lat + ", " + deserializedData.coord.lon + "\n";
                data += "Weather: " + deserializedData.weather[0].main + ", " + deserializedData.weather[0].description + "\n";
                data += "Temperature: " + deserializedData.main.temp + "°C, " + "Feels Like: " + deserializedData.main.feels_like + "°C\n";
                data += "Pressure: " + deserializedData.main.pressure + "hPa\n" + "Humidity: " + deserializedData.main.humidity + "%\n";
                data += "Wind speed: " + deserializedData.wind.speed + "m/s\n";
                data += "Sunrise: " + UnixTimeStampToDateTime(deserializedData.sys.sunrise) + "\n";
                data += "Sunset: " + UnixTimeStampToDateTime(deserializedData.sys.sunset) + "\n";
                Test_Json_Data.Text = data;
            } catch(Exception ex)
            {
                MessageBox.Show("Incorrect city name!\nErrorLog:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        /// <summary>
        /// Odpowiedź na kliknięcie przycisku dodania do ulubionych, dodaje miasto do listy ulubionych miast
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_To_Fav(object sender, RoutedEventArgs e)
        {
            try
            {
                    IList<FavouriteCity> city;
                    //WeatherData wthrDt = WeatherData.download_weather_to_weather_data_object(current_city);
                    using (var context = new FavouriteContext())
                    {
                    FavouriteCity c = new FavouriteCity()
                    {
                        ID = 1,
                        Name = current_city,
                        WeatherData = WeatherData.download_weather(current_city),
                        date = DateTime.Now,
                        onStart = false
                        };
                    if (!context.city_info.Any(x => x.Name == current_city))
                    {
                        context.city_info.Add(c);
                        context.SaveChanges();
                        city = context.city_info.ToList();
                        FavouriteCities.ItemsSource = city;
                    }
                    else
                        MessageBox.Show("This city is already added to favouries!", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Incorrect city name!\nErrorLog:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Odpowiedź na kliknięcie przycisku pokazania pogody wybranego miasta, wyświetla pogodę z listy miast ulubionych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_Selected_Click(object sender, RoutedEventArgs e)
        {
            var favCity = FavouriteCities.SelectedItem as FavouriteCity;
            if (favCity != null)
            {
                string cityName = (FavouriteCities.SelectedItem as FavouriteCity).Name;
                //selected = WeatherData.download_weather_to_weather_data_object((FavouriteCities.SelectedItem as FavouriteCity).Name);
                using (var context = new FavouriteContext())
                {
                    var query = context.city_info.Where(x => x.Name == cityName).SingleOrDefault<FavouriteCity>();
                    var selected = JsonConvert.DeserializeObject<WeatherData>(query.WeatherData);
                    string data = "City: " + cityName + "\n";
                    data += "Coords: " + selected.coord.lat + ", " + selected.coord.lon + "\n";
                    data += "Weather: " + selected.weather[0].main + ", " + selected.weather[0].description + "\n";
                    data += "Temperature: " + selected.main.temp + "°C, " + "Feels Like: " + selected.main.feels_like + "°C\n";
                    data += "Pressure: " + selected.main.pressure + "hPa\n" + "Humidity: " + selected.main.humidity + "%\n";
                    data += "Wind speed: " + selected.wind.speed + "m/s\n";
                    data += "Sunrise: " + UnixTimeStampToDateTime(selected.sys.sunrise) + "\n";
                    data += "Sunset: " + UnixTimeStampToDateTime(selected.sys.sunset) + "\n";
                    data += "Last checked: " + query.date;
                    FavInfo.Text = data;
                }
            }

        }
        /// <summary>
        /// Odpowiedź na kliknięcie przycisku usunięcia z ulubionych, usuwa miasto z listy ulubionych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_Selected_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FavouriteContext())
            {
                FavouriteCity itemToRemove = null;
                if (FavouriteCities.SelectedItem != null)
                {
                    int idToRemove = (FavouriteCities.SelectedItem as FavouriteCity).ID;
                    itemToRemove = context.city_info.SingleOrDefault(x => x.ID == idToRemove);
                }
                IList<FavouriteCity> city;
                if (itemToRemove != null)
                {
                    context.city_info.Remove(itemToRemove);
                    context.SaveChanges();
                    city = context.city_info.ToList();
                    FavouriteCities.ItemsSource = city;
                }
                
            }
        }
        /// <summary>
        /// Odpowiedź na kliknięcie przycisku pokazania na start, wybiera miasto z ulubionych, które pokazywane będzie na ekranie głównym aplikacji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_On_Start(object sender, RoutedEventArgs e)
        {
            using (var context = new FavouriteContext())
            {
                if (FavouriteCities.SelectedItem != null)
                {
                    //Resetujemy wartość na start
                    foreach (FavouriteCity city_entry in context.city_info)
                    {
                        context.Entry(city_entry).Entity.onStart = false;
                    }
                    int idOnStart = (FavouriteCities.SelectedItem as FavouriteCity).ID;
                    var result = context.city_info.SingleOrDefault(x => x.ID == idOnStart);
                    result.onStart = true;
                    context.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Odpowiedź na kliknięcie przycisku aktualizacji pogody ulubionych miast, aktualizuje pogodę na teraźniejszą
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fav_Update(object sender, RoutedEventArgs e)
        {
            using (var context = new FavouriteContext())
            {
                foreach (FavouriteCity city_entry in context.city_info)
                {
                    context.Entry(city_entry).Entity.WeatherData = WeatherData.download_weather(city_entry.Name);
                    context.Entry(city_entry).Entity.date = DateTime.Now;
                }
                context.SaveChanges();
            }
        }
    }


}
