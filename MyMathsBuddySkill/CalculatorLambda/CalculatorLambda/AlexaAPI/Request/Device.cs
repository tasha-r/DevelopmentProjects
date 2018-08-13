using Newtonsoft.Json;
using System.Collections.Generic;

namespace CalculatorLambda.AlexaAPI.Request
{
    public class Device
    {
        [JsonProperty("supportedInterfaces")]
        public Dictionary<string, object> SupportedInterfaces { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceID { get; set; }
    }
}
