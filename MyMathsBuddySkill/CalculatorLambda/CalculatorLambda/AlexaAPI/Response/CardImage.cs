using Newtonsoft.Json;

namespace CalculatorLambda.AlexaAPI.Response
{
    public class CardImage
    {
        [JsonProperty("smallImageUrl")]
        public string SmallImageUrl { get; set; }

        [JsonProperty("largeImageUrl")]
        public string LargeImageUrl { get; set; }
    }
}
