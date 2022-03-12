using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Moq;
using System;
using Xunit;
namespace MetricsAgentTests
{
    public class RamMetricsControllerUnitTests
    {
        private RamMetricsController controller;
        private Mock<IRamMetricsRepository> mock;
        public RamMetricsControllerUnitTests()
        {
            mock = new Mock<IRamMetricsRepository>();
            controller = new RamMetricsController(mock.Object);
        }
        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // ������������� �������� ��������
            // � �������� �����������, ��� � ����������� �������� RamMetric - ������
            mock.Setup(repository => repository.Create(It.IsAny<RamMetric>())).Verifiable();
            // ��������� �������� �� �����������
            var result = controller.Create(new MetricsAgent.Requests.RamMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });
            // ��������� �������� �� ��, ��� ���� ������� ����������
            // �������� ����� Create ����������� � ������ ����� ������� � ���������
        mock.Verify(repository => repository.Create(It.IsAny<RamMetric>()),Times.AtMostOnce());
        }
    }
}