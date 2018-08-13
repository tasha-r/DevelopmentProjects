using Newtonsoft.Json;

namespace CalculatorLambda.AlexaAPI.Response
{
    public class Reprompt
    {
        [JsonProperty("outputSpeech", NullValueHandling = NullValueHandling.Ignore)]
        public IOutputSpeech OutputSpeech { get; set; }
    }
}
