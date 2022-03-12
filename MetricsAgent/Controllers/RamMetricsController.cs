using AutoMapper;
using MetricsAgent.DAL;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly IRamMetricsRepository repository;
        private readonly IMapper mapper;
        public RamMetricsController(IRamMetricsRepository repository, IMapper
        mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IList<RamMetric> metrics = repository.GetAll();
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<RamMetricDto>(metric));
            }
            return Ok(response);
        }
    }
}