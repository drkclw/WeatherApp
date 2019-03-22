using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Models
{
    public class Wind
    {
        public double Speed { get; set; }

        [JsonProperty("deg")]
        public int Degrees { get; set; }

        public double Gust { get; set; }
    }
}
