using Newtonsoft.Json;

namespace CalculatorLambda.AlexaAPI.Request
{
    public class Slot
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
