using MetricsAgent.DAL;
using Quartz;
using System.Diagnostics;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private IRamMetricsRepository _repository;
        // Счётчик для метрики CPU
        private PerformanceCounter _cpuCounter;
        public RamMetricJob(IRamMetricsRepository repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }
        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости CPU
            var cpuUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());
            // Узнаем, когда мы сняли значение метрики
            var time =
            TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _repository.Create(new Models.RamMetric
            {
                Time = time,
                Value = cpuUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}