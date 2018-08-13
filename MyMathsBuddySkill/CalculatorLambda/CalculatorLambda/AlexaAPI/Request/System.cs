using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorLambda.AlexaAPI.Request
{
    public class System
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("device")]
        public Device Device { get; set; }

        [JsonProperty("apiEndpoint")]
        public string ApiEndpoint { get; set; }
    }
}
