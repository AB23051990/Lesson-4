using AutoMapper;
using MetricsAgent.DAL;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkMetricsController : ControllerBase
    {
        private readonly INetworkMetricsRepository repository;
        private readonly IMapper mapper;
        public NetworkMetricsController(INetworkMetricsRepository repository, IMapper
        mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IList<NetworkMetric> metrics = repository.GetAll();
            var response = new AllNetworkMetricsResponse()
            {
                Metrics = new List<NetworkMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<NetworkMetricDto>(metric));
            }
            return Ok(response);
        }
    }
}