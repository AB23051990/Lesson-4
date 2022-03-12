using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Moq;
using System;
using Xunit;
namespace MetricsAgentTests
{
    public class HddMetricsControllerUnitTests
    {
        private HddMetricsController controller;
        private Mock<IHddMetricsRepository> mock;
        public HddMetricsControllerUnitTests()
        {
            mock = new Mock<IHddMetricsRepository>();
            controller = new HddMetricsController(mock.Object);
        }
        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // Устанавливаем параметр заглушки
            // В заглушке прописываем, что в репозиторий прилетит HddMetric - объект
            mock.Setup(repository => repository.Create(It.IsAny<HddMetric>())).Verifiable();
            // Выполняем действие на контроллере
            var result = controller.Create(new MetricsAgent.Requests.HddMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });
            // Проверяем заглушку на то, что пока работал контроллер
            // Вызвался метод Create репозитория с нужным типом объекта в параметре
        mock.Verify(repository => repository.Create(It.IsAny<HddMetric>()),Times.AtMostOnce());
        }
    }
}