using AutoMapper;
using MetricsAgent.DAL;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly IDotNetMetricsRepository repository;
        private readonly IMapper mapper;
        public DotNetMetricsController(IDotNetMetricsRepository repository, IMapper
        mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IList<DotNetMetric> metrics = repository.GetAll();
            var response = new AllDotNetMetricsResponse()
            {
                Metrics = new List<DotNetMetricDto>()
            };
            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<DotNetMetricDto>(metric));
            }
            return Ok(response);
        }
    }
}