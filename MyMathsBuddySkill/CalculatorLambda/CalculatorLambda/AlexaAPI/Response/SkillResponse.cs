using Newtonsoft.Json;
using System.Collections.Generic;

namespace CalculatorLambda.AlexaAPI.Response
{
    public class SkillResponse
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("sessionAttributes", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> SessionAttributes { get; set; }

        [JsonProperty("response")]
        public ResponseBody ResponseBody { get; set; }
    }
}
