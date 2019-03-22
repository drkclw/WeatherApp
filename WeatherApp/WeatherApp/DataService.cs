using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp
{
    public class DataService
    {
        public static async Task<WeatherResponse> GetDataFromService(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            WeatherResponse data = null;
            try
            {
                if (response != null)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<WeatherResponse>(json);
                }
            }catch(Exception ex)
            {
                data.CityName = ex.Message;
            }

            return data;
        }
    }
}
