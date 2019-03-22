using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Models
{
    public class Main
    {
        [JsonProperty("temp")]
        public double Temperature { get; set; }

        public int Pressure { get; set; }

        public int Humidity { get; set; }

        [JsonProperty("temp_min")]
        public double Low { get; set; }

        [JsonProperty("temp_max")]
        public double High { get; set; }
    }
}
