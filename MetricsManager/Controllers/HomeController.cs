using Microsoft.AspNetCore.Mvc;
using NLog;

namespace MetricsManager.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            // логируем, что мы пошли в соседний сервис
            Logger.LogInformation($"starting new request to metrics agent");
            // обращение в сервис
            var metrics = metricsAgentClient.GetCpuMetrics(new GetAllCpuMetricsRequest
            {
                FromTime = fromTime,
                ToTime = toTime
            });
            // возвращаем ответ
            return Ok(metrics);
        }
    }
}
