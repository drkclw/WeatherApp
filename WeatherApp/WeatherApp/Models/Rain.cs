using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Models
{
    public class Rain
    {
        [JsonProperty("1h")]
        public string LastHour { get; set; }

        [JsonProperty("3h")]
        public string LastThreeHours { get; set; }
    }
}
