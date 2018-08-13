using Amazon.Lambda.Core;
using CalculatorLambda.AlexaAPI.Request;
using CalculatorLambda.AlexaAPI.Response;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace CalculatorLambda.Tests
{
    public class FunctionTests
    {
        [Fact]
        public void FunctionHandler_WhenCalled_SetsDefaultValues()
        {
            Setup(out Mock<ILambdaContext> context, out SkillRequest request, "LaunchRequest");

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.False(response.ResponseBody.ShouldEndSession);
            Assert.Equal("en-GB", response.SessionAttributes["locale"]);
            Assert.Equal("1.0", response.Version);
        }

        [Fact]
        public void FunctionHandler_WhenCalledAndAnExceptionOccurs_ReturnsNull()
        {
            Setup(out Mock<ILambdaContext> context, out SkillRequest request);

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.Null(response);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithLaunchRequestAndLocaleSet_ReturnsResponseWithSpecifiedLocale()
        {
            Setup(out Mock<ILambdaContext> context, out SkillRequest request, "LaunchRequest");
            request.RequestBody.Locale = "en-US";

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.Equal("en-US", response.SessionAttributes["locale"]);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithLaunchRequest_ReturnsResponseWithMessageSet()
        {
            Setup(out Mock<ILambdaContext> context, out SkillRequest request, "LaunchRequest");

            var response = new Function().FunctionHandler(request, context.Object);

            var launchMessage = (SsmlOutputSpeech)response.ResponseBody.OutputSpeech;
            Assert.Equal(
                "<speak>Welcome to My Maths Buddy. I know how to add, multiply, divide and subtract two numbers... What can I help you with?</speak>",
                launchMessage.Ssml);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithLaunchRequest_ReturnsResponseWithRepromptSet()
        {
            Setup(out Mock<ILambdaContext> context, out SkillRequest request, "LaunchRequest");

            var response = new Function().FunctionHandler(request, context.Object);

            var repromptMessage = (PlainTextOutputSpeech)response.ResponseBody.Reprompt.OutputSpeech;
            Assert.Equal(
                "Try asking me to add two numbers together.",
                repromptMessage.Text);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithIntentRequestAndLocaleSet_ReturnsResponseWithSpecifiedLocale()
        {
            SetupIntentRequest(out Mock<ILambdaContext> context, out SkillRequest request, "AMAZON.CancelIntent");
            request.Session.Attributes = new Dictionary<string, object> { { "locale", "en-US" } };

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.Equal("en-US", response.SessionAttributes["locale"]);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithDialogIntentRequestAndSequenceIsNotComplete_SetsDialogDelegateDirective()
        {
            SetupIntentRequest(out Mock<ILambdaContext> context, out SkillRequest request, "AddNumbers", "INCOMPLETE");

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.Equal("Dialog.Delegate", response.ResponseBody.Directives[0].Type);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithDialogIntentRequestWithNoNumbers_ReturnsResponseWithHelpRepromptMessageSet()
        {
            SetupIntentRequest(out Mock<ILambdaContext> context, out SkillRequest request, "AddNumbers", "COMPLETED");

            var response = new Function().FunctionHandler(request, context.Object);

            var outputMessage = (PlainTextOutputSpeech)response.ResponseBody.OutputSpeech;
            Assert.Equal("What can I help you with?", outputMessage.Text);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithAddNumbersDialogIntentRequestWithInvalidNumber_TreatsNumberAsZeroAndReturnsResultInResponseMessage()
        {
            SetupIntentRequest(out Mock<ILambdaContext> context, out SkillRequest request, "AddNumbers", "COMPLETED");
            request.RequestBody.Intent.Slots = new Dictionary<string, Slot>
            {
                { "NumberOne", new Slot { Name = "NumberOne", Value = "Hello" } },
                { "NumberTwo", new Slot { Name = "NumberTwo", Value = "4" } },
            };

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.Equal("<speak>The result of 4 plus zero is 4.</speak>", ((SsmlOutputSpeech)response.ResponseBody.OutputSpeech).Ssml);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithAddNumbersDialogIntentRequestWithNumbers_ReturnsSumOfNumbersInMessage()
        {
            SetupIntentRequest(out Mock<ILambdaContext> context, out SkillRequest request, "AddNumbers", "COMPLETED");
            request.RequestBody.Intent.Slots = new Dictionary<string, Slot>
            {
                { "NumberOne", new Slot { Name = "NumberOne", Value = "4" } },
                { "NumberTwo", new Slot { Name = "NumberTwo", Value = "6" } },
            };

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.Equal("<speak>The result of 4 plus 6 is 10.</speak>", ((SsmlOutputSpeech)response.ResponseBody.OutputSpeech).Ssml);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithCancelIntentRequest_ReturnsResponseWithFlagAndMessageSet()
        {
            SetupIntentRequest(out Mock<ILambdaContext> context, out SkillRequest request, "AMAZON.CancelIntent");

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.True(response.ResponseBody.ShouldEndSession);
            Assert.Equal("Goodbye!", ((PlainTextOutputSpeech)response.ResponseBody.OutputSpeech).Text);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithStopIntentRequest_ReturnsResponseWithFlagAndMessageSet()
        {
            SetupIntentRequest(out Mock<ILambdaContext> context, out SkillRequest request, "AMAZON.StopIntent");

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.True(response.ResponseBody.ShouldEndSession);
            Assert.Equal("Goodbye!", ((PlainTextOutputSpeech)response.ResponseBody.OutputSpeech).Text);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithHelpIntentRequest_ReturnsResponseWithMessageSet()
        {
            SetupIntentRequest(out Mock<ILambdaContext> context, out SkillRequest request, "AMAZON.HelpIntent");

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.Equal("You can ask me to add, multiply, divide or subtract two numbers.", ((PlainTextOutputSpeech)response.ResponseBody.OutputSpeech).Text);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithFallbackIntentRequest_ReturnsResponseWithMessageSet()
        {
            SetupIntentRequest(out Mock<ILambdaContext> context, out SkillRequest request, "AMAZON.FallbackIntent");

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.Equal("Hmm... I don't know how to do that... Sorry.", ((PlainTextOutputSpeech)response.ResponseBody.OutputSpeech).Text);
        }

        [Fact]
        public void FunctionHandler_WhenCalledWithUnknownIntentRequest_ReturnsResponseWithDefaultMessageSet()
        {
            SetupIntentRequest(out Mock<ILambdaContext> context, out SkillRequest request, "Unknown intent");

            var response = new Function().FunctionHandler(request, context.Object);

            Assert.Equal("What can I help you with?", ((PlainTextOutputSpeech)response.ResponseBody.OutputSpeech).Text);
        }

        private void Setup(
            out Mock<ILambdaContext> context,
            out SkillRequest request,
            string requestType = null)
        {
            context = new Mock<ILambdaContext>();
            context.Setup(c => c.Logger).Returns(new Mock<ILambdaLogger>().Object);

            request = new SkillRequest
            {
                RequestBody = new RequestBody
                {
                    Type = requestType,
                },
            };
        }

        private void SetupIntentRequest(
            out Mock<ILambdaContext> context,
            out SkillRequest request,
            string intentName,
            string dialogState = null)
        {
            Setup(out context, out request, "IntentRequest");
            request.RequestBody.DialogState = dialogState;
            request.Session = new Session();
            request.RequestBody.Intent = new Intent
            {
                Name = intentName,
            };
        }
    }
}
