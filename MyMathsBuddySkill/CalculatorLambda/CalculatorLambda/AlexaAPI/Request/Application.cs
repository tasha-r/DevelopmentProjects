using Newtonsoft.Json;

namespace CalculatorLambda.AlexaAPI.Request
{
    public class Application
    {
        [JsonProperty("applicationID")]
        public string ApplicationId { get; set; }
    }
}
