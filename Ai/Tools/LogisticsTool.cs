using ok.Service;

namespace ok.Ai.Tools
{
    public class LogisticsTool
    {
        private readonly LogisticsService _service;

        public LogisticsTool(LogisticsService service)
        {
            _service = service;
        }

        public string Execute(int scladId)
        {
            var result = _service.DistributeFromWarehouse(scladId);

            return string.Join("\n", result.Select(x =>
                $"Point {x.Key}: {x.Value}"));
        }
    }
}
