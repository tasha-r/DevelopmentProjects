using System;
using System.Collections.Generic;
using Amazon.Lambda.Core;
using CalculatorLambda.AlexaAPI;
using CalculatorLambda.AlexaAPI.Request;
using CalculatorLambda.AlexaAPI.Response;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace CalculatorLambda
{
    public class Function
    {
        const string GB_LOCALE = "en-GB";
        const string LOCALENAME = "locale";
        const string NUMBERONE = "NumberOne";
        const string NUMBERTWO = "NumberTwo";

        private ILambdaContext _context;
        private SkillRequest _request;
        private SkillResponse _response;
        private RequestBody _requestBody;

        /// <summary>
        /// A function that receives a request and context from an Alexa skill and
        /// handles the request.
        /// </summary>
        /// <param name="_request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SkillResponse FunctionHandler(SkillRequest request, ILambdaContext context)
        {
            _context = context;
            _request = request;
            _requestBody = _request.RequestBody;

            try
            {
                var responseBody = new ResponseBody
                {
                    ShouldEndSession = false,
                };

                _response = new SkillResponse
                {
                    ResponseBody = responseBody,
                    Version = AlexaConstants.AlexaVersion,
                };

                HandleRequest();

                Log(JsonConvert.SerializeObject(_response));
                return _response;
            }
            catch (Exception e)
            {
                Log($"Error: {e.Message}");
            }

            return null;
        }

        /// <summary>
        /// Handle the request based on it's type.
        /// </summary>
        /// <returns>void</returns>
        private void HandleRequest()
        {
            // Set locale
            var locale = _requestBody.Locale;
            if (string.IsNullOrEmpty(locale))
            {
                locale = GB_LOCALE;
            }

            var skillData = new SkillData(locale);

            if (_requestBody.Type.Equals(AlexaConstants.LaunchRequest))
            {
                // Handle skill launch request
                ProcessLaunchRequest(skillData);
                _response.SessionAttributes = new Dictionary<string, object>() { { LOCALENAME, skillData.Locale } };
            }
            else if (_requestBody.Type.Equals(AlexaConstants.IntentRequest))
            {
                HandleIntentRequest(skillData);
            }
        }

        /// <summary>
        /// Handle request of type intent
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns>void</returns>
        private void HandleIntentRequest(SkillData skillData)
        {
            if (_request.Session.Attributes != null)
            {
                var sessionLocale = _request.Session.Attributes[LOCALENAME].ToString();
                if (!string.IsNullOrEmpty(sessionLocale))
                {
                    skillData.Locale = sessionLocale;
                }
            }

            _response.SessionAttributes = new Dictionary<string, object>() { { LOCALENAME, skillData.Locale } };

            if (IsDialogIntentRequest() && !IsDialogSequenceComplete())
            {
                CreateDelegateResponse();
            }
            else if (!ProcessDialogIntentRequest(skillData))
            {
                _response.ResponseBody.OutputSpeech = ProcessIntentRequest(skillData);
            }
        }

        /// <summary>
        /// Process intents that are not dialog based and have 
        /// a speech response.
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns>IOutputSpeech</returns>
        private IOutputSpeech ProcessIntentRequest(SkillData skillData)
        {
            var responseText = string.Empty;

            switch (_requestBody.Intent.Name)
            {
                case AlexaConstants.CancelIntent:
                    responseText = skillData.StopMessage;
                    _response.ResponseBody.ShouldEndSession = true;
                    break;
                case AlexaConstants.StopIntent:
                    responseText = skillData.StopMessage;
                    _response.ResponseBody.ShouldEndSession = true;
                    break;
                case AlexaConstants.HelpIntent:
                    responseText = skillData.HelpMessage;
                    break;
                case AlexaConstants.FallbackIntent:
                    responseText = skillData.FallbackMessage;
                    break;
                default:
                    responseText = skillData.HelpReprompt;
                    break;
            }

            return new PlainTextOutputSpeech
            {
                Text = responseText,
            };
        }

        /// <summary>
        /// Process and respond to the LaunchRequest with launch
        /// and reprompt message
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns>void</returns>
        private void ProcessLaunchRequest(SkillData skillData)
        {
            var innerResponse = new SsmlOutputSpeech
            {
                Ssml = SsmlDecorate(skillData.LaunchMessage),
            };

            _response.ResponseBody.OutputSpeech = innerResponse;

            var prompt = new PlainTextOutputSpeech
            {
                Text = skillData.LaunchMessageReprompt,
            };

            _response.ResponseBody.Reprompt = new Reprompt
            {
                OutputSpeech = prompt,
            };
        }

        /// <summary>
        ///  Process intents that are dialog based and may not have a speech
        ///  response. Speech responses cannot be returned with a delegate response
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns>bool true if processed</returns>
        private bool ProcessDialogIntentRequest(SkillData skillData)
        {
            var speechMessage = string.Empty;
            var processed = false;
            var numbers = GetNumbers();

            if (numbers == null)
            {
                return false;
            }

            switch(_requestBody.Intent.Name)
            {
                case "AddNumbers":
                    speechMessage = AddNumbers(skillData, numbers);
                    break;
                case "MultiplyNumbers":
                    speechMessage = MultiplyNumbers(skillData, numbers);
                    break;
                default:
                    Log($"Intent name not matched to anything. Intent name: {_requestBody.Intent.Name}");
                    break;
            }

            if (!string.IsNullOrEmpty(speechMessage))
            {
                _response.ResponseBody.OutputSpeech = new SsmlOutputSpeech
                {
                    Ssml = SsmlDecorate(speechMessage),
                };

                processed = true;
            }

            return processed;
        }

        /// <summary>
        /// Check if the request is a dialog intent request, e.g. part of a dialog sequence
        /// </summary>
        /// <returns>bool true if a dialog</returns>
        private bool IsDialogIntentRequest()
        {
            return !string.IsNullOrEmpty(_requestBody.DialogState);
        }

        /// <summary>
        /// Check if the request dialog sequence is complete
        /// </summary>
        /// <param name="input"></param>
        /// <returns>bool true if dialog complete set</returns>
        private bool IsDialogSequenceComplete()
        {
            return _requestBody.DialogState.Equals(AlexaConstants.DialogCompleted);
        }

        /// <summary>
        ///  Create a delegate response. All dialog requests 
        ///  except "Complete" are delegated
        /// </summary>
        /// <returns>void</returns>
        private void CreateDelegateResponse()
        {
            var dialogDirective = new DialogDirective
            {
                Type = AlexaConstants.DialogDelegate,
            };

            _response.ResponseBody.Directives.Add(dialogDirective);
        }

        /// <summary>
        ///  Prepare text for Ssml output
        /// </summary>
        /// <param name="speech"></param>
        /// <returns>string</returns>
        private string SsmlDecorate(string speech)
        {
            return $"<speak>{speech}</speak>";
        }

        /// <summary>
        ///  Retrieve the two numbers from the request
        /// </summary>
        /// <param name="speech"></param>
        /// <returns>string</returns>
        private List<decimal> GetNumbers()
        {
            var slots = _requestBody.Intent.Slots;

            if (slots == null)
            {
                return null;
            }

            var numbers = new List<decimal>();
            GetNumber(slots, NUMBERONE, numbers);
            GetNumber(slots, NUMBERTWO, numbers);

            return numbers;
        }

        /// <summary>
        ///  Retrieve a number from the request intent slots
        /// </summary>
        /// <param name="slots"></param>
        /// <param name="key"></param>
        /// <param name="numbers"></param>
        /// <returns>void</returns>
        private void GetNumber(Dictionary<string, Slot> slots, string key, List<decimal> numbers)
        {
            if (slots.ContainsKey(key))
            {
                if (slots.TryGetValue(key, out Slot numberSlot)
                    && (decimal.TryParse(numberSlot.Value, out decimal number)))
                {
                    numbers.Add(number);
                }
            }
        }

        /// <summary>
        ///  Add numbers together. Delegate to the dialog
        ///  if the Complete protocol flag is not set
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns>string weather newfact or empty string</returns>
        private string AddNumbers(SkillData skillData, List<decimal> numbers)
        {
            var speechMessage = string.Empty;
            switch (numbers.Count)
            {
                case 0:
                    speechMessage = $"The result of zero plus zero equals zero. ";
                    break;
                case 1:
                    speechMessage = $"The result of {numbers[0]} plus zero is {numbers[0]}.";
                    break;
                case 2:
                    var result = Calculator.Add(numbers[0], numbers[1]);
                    speechMessage = $"The result of {numbers[0]} plus {numbers[1]} is {result}.";
                    break;
                default:
                    speechMessage = "Sorry, something went wrong. Please try again. ";
                    break;
            }

            return speechMessage;
        }

        /// <summary>
        ///  Multiply numbers together. Delegate to the dialog
        ///  if the Complete protocol flag is not set
        /// </summary>
        /// <param name="skillData"></param>
        /// <returns>string weather newfact or empty string</returns>
        private string MultiplyNumbers(SkillData skillData, List<decimal> numbers)
        {
            var speechMessage = string.Empty;
            switch (numbers.Count)
            {
                case 0:
                    speechMessage = $"The result of zero multiplied by zero equals zero. ";
                    break;
                case 1:
                    speechMessage = $"The result of {numbers[0]} multiplied by zero is zero.";
                    break;
                case 2:
                    var result = Calculator.Multiply(numbers[0], numbers[1]);
                    speechMessage = $"The result of {numbers[0]} multiplied by {numbers[1]} is {result}.";
                    break;
                default:
                    speechMessage = "Sorry, something went wrong. Please try again. ";
                    break;
            }

            return speechMessage;
        }

        private void Log(string text)
        {
            _context.Logger.Log(text);
        }
    }
}
