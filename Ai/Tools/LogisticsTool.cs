using ok.Service;

namespace ok.Ai.Tools
{
    public class LogisticsTool
    {
        private readonly LogisticsService _service;
        private readonly LlmService _llm;

        public LogisticsTool(LogisticsService service, LlmService llm)
        {
            _service = service;
            _llm = llm;
        }

        public LogisticsService GetService() => _service;

        public async Task<string> ExecuteAsync(int scladId)
        {

            var result = _service.DistributeFromWarehouse(scladId);

            if (result.Points.Count == 0)
                return "Немає попиту.";

            var lines = new List<string>();
            lines.Add($"Склад: {result.WarehouseName}\n");
            foreach (var p in result.Points)
            {
                lines.Add(
                    $"{p.Name} (ID {p.PointId}): потрібно {p.Required}, отримано {p.Allocated}"
                );
            }

            lines.Add($"\nЗалишок на складі: {result.RemainingStock}");

            var prompt = $@"
            Є логістичний розподіл ресурсів:
            {string.Join("\n", lines)}
            Поясни коротко українською:
            чому ресурси розподілені саме так.
            ";

            var aiExplanation = await _llm.GenerateAsync(prompt);

            lines.Add("\nAI пояснення:");
            lines.Add(aiExplanation);

            return string.Join("\n", lines);
        }
    }
}