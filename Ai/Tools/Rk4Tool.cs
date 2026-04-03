using ok.Service;
//using Microsoft.Agents.AI.Executors;
namespace ok.Ai.Tools
{
    public class Rk4Tool
    {
        private readonly Rk4Service _service;

        public Rk4Tool(Rk4Service service)
        {
            _service = service;
        }

        public string Execute(int scladId, double coef, double time)
        {
            var result = _service.RungeKutta4(scladId, coef, time);
            return $"Прогноз залишків: {result}";
        }
    }
}