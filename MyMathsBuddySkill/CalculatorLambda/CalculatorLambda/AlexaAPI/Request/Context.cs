using Newtonsoft.Json;

namespace CalculatorLambda.AlexaAPI.Request
{
    public class Context
    {
        [JsonProperty("System")]
        public System System { get; set; }
    }
}
