﻿using Newtonsoft.Json;

namespace CalculatorLambda.AlexaAPI.Response
{
    public class SsmlOutputSpeech : IOutputSpeech
    {
        [JsonRequired]
        [JsonProperty("type")]
        public string Type { get { return "SSML"; } }

        [JsonRequired]
        [JsonProperty("ssml")]
        public string Ssml { get; set; }
    }
}
