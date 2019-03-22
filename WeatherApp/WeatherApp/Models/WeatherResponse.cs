using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Models
{
    public class WeatherResponse
    {
        [JsonProperty("coord")]
        public Coordinates Coordinates { get; set; }

        public IList<WeatherType> Weather { get; set; }

        public Main Main { get; set; }

        public int Visibility { get; set; }

        public Wind Wind { get; set; }

        public Snow Snow { get; set; }

        public Clouds Clouds { get; set; }

        [JsonProperty("dt")]
        public long WeatherDateTime { get; set; }

        public Sys Sys { get; set; }

        [JsonProperty("id")]
        public long CityId { get; set; }

        [JsonProperty("name")]
        public string CityName { get; set; }
    }
}
