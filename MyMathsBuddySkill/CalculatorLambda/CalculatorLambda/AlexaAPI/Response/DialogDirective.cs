using Newtonsoft.Json;

namespace CalculatorLambda.AlexaAPI.Response
{
    public class DialogDirective : IDirective
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
