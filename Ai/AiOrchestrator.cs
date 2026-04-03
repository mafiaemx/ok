using System.Text.RegularExpressions;
using ok.Ai.Tools;
using ok.Service;

namespace ok.Ai
{
    public enum Intent
    {
        Logistics,
        Forecast,
        General
    }

    public class AiOrchestrator
    {
        private readonly LogisticsTool _logisticsTool;
        private readonly LlmService _llm;

        public AiOrchestrator(LogisticsTool logisticsTool, LlmService llm)
        {
            _logisticsTool = logisticsTool;
            _llm = llm;
        }

        public async Task<string> RunAsync(string prompt)
        {
            var intent = DetectIntent(prompt);

            switch (intent)
            {
                case Intent.Logistics:
                    int scladId = ExtractScladId(prompt);
                    return await _logisticsTool.ExecuteAsync(scladId);

                case Intent.Forecast:
                    return "Модуль прогнозування ще не реалізований";

                default:
                    return await _llm.GenerateAsync(prompt);
            }
        }

        private Intent DetectIntent(string prompt)
        {
            prompt = prompt.ToLower();

            if (prompt.Contains("розподіл") ||
                prompt.Contains("доставка") ||
                prompt.Contains("склад"))
                return Intent.Logistics;

            if (prompt.Contains("прогноз"))
                return Intent.Forecast;

            return Intent.General;
        }

        private int ExtractScladId(string prompt)
        {
            var match = Regex.Match(prompt, @"\d+");

            if (match.Success)
                return int.Parse(match.Value);

            return 1; 
        }
    }
}