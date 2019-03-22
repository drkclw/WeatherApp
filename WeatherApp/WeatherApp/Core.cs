using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class Core
    {
        public static async Task<Weather> GetWeatherByZipcode(string zipCode)
        {            
            string key = "d5f21f686e7430db18ec078781c0ec43";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?zip="
                + zipCode + ",us&appid=" + key + "&units=imperial";
           
            var result = await DataService.GetDataFromService(queryString).ConfigureAwait(false);

            if (result.Weather != null)
            {
                Weather weather = new Weather();
                weather.CityName = result.CityName;
                weather.Temperature = result.Main.Temperature + " F";
                weather.Wind = result.Wind.Speed + " mph";
                weather.Humidity = result.Main.Humidity + " %";
                weather.Visibility = result.Visibility.ToString();

                DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime sunrise = time.AddSeconds(result.Sys.Sunrise);
                DateTime sunset = time.AddSeconds(result.Sys.Sunset);
                weather.Sunrise = result.Sys.Sunrise.ToString() + " UTC";
                weather.Sunset = result.Sys.Sunset.ToString() + " UTC";
                weather.IconUrl = "http://openweathermap.org/img/w/" + result.Weather[0].Icon + ".png";
                weather.WeatherDescription = result.Weather[0].Description;
                return weather;
            }
            else
            {
                return null;
            }
        }

        public static async Task<Weather> GetWeatherByCoordinates(double latitude, double longitude)
        {            
            string key = "d5f21f686e7430db18ec078781c0ec43";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?lat="
                + latitude + "&lon=" + longitude + "&appid=" + key + "&units=imperial";
           
            var result = await DataService.GetDataFromService(queryString).ConfigureAwait(false);

            if (result.Weather != null)
            {
                Weather weather = new Weather();
                weather.CityName = result.CityName;
                weather.Temperature = result.Main.Temperature + " F";
                weather.Wind = result.Wind.Speed + " mph";
                weather.Humidity = result.Main.Humidity + " %";
                weather.Visibility = result.Visibility.ToString();

                DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime sunrise = time.AddSeconds(result.Sys.Sunrise);
                DateTime sunset = time.AddSeconds(result.Sys.Sunset);
                weather.Sunrise = result.Sys.Sunrise.ToString() + " UTC";
                weather.Sunset = result.Sys.Sunset.ToString() + " UTC";
                weather.IconUrl = "http://openweathermap.org/img/w/" + result.Weather[0].Icon + ".png";
                weather.WeatherDescription = result.Weather[0].Description;
                return weather;
            }
            else
            {
                return null;
            }
        }
    }
}
