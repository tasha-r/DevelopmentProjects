namespace CalculatorLambda
{
    public class SkillData
    {
        public string Locale { get; set; }

        public string SkillName { get; set; }

        public string HelpMessage { get; set; }

        public string HelpReprompt { get; set; }

        public string FallbackMessage { get; set; }

        public string StopMessage { get; set; }

        public string LaunchMessage { get; set; }

        public string LaunchMessageReprompt { get; set; }

        public SkillData(string locale)
        {
            Locale = locale;
            SkillName = "My Maths Buddy";
            LaunchMessage = $"Welcome to {SkillName}. I know how to add, multiply, divide and subtract two numbers... What can I help you with?";
            LaunchMessageReprompt = "Try asking me to add two numbers together.";
            HelpMessage = "You can ask me to add, multiply, divide or subtract two numbers.";
            HelpReprompt = "What can I help you with?";
            FallbackMessage = "Hmm... I don't know how to do that... Sorry.";
            StopMessage = "Goodbye!";
        }
    }
}
