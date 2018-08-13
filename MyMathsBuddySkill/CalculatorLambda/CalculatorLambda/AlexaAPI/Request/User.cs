using Newtonsoft.Json;

namespace CalculatorLambda.AlexaAPI.Request
{
    public class User
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}
