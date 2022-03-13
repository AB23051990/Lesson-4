using MetricsAgent.DAL;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class HddMetricJob : IJob
    {
        private IHddMetricsRepository _repository;
        // Счётчик для метрики CPU
        private PerformanceCounter _cpuCounter;
        public HddMetricJob(IHddMetricsRepository repository)
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
            _repository.Create(new Models.HddMetric
            {
                Time = time,
                Value = cpuUsageInPercents
            });
            return Task.CompletedTask;
        }
    }
}