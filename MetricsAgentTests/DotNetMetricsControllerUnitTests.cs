using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Moq;
using System;
using Xunit;
namespace MetricsAgentTests
{
    public class DotNetMetricsControllerUnitTests
    {
        private DotNetMetricsController controller;
        private Mock<IDotNetMetricsRepository> mock;
        public DotNetMetricsControllerUnitTests()
        {
            mock = new Mock<IDotNetMetricsRepository>();
            controller = new DotNetMetricsController(mock.Object);
        }
        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // ������������� �������� ��������
            // � �������� �����������, ��� � ����������� �������� DotNetMetric - ������
            mock.Setup(repository => repository.Create(It.IsAny<DotNetMetric>())).Verifiable();
            // ��������� �������� �� �����������
            var result = controller.Create(new MetricsAgent.Requests.DotNetMetricCreateRequest
            {
                Time = TimeSpan.FromSeconds(1),
                Value = 50
            });
            // ��������� �������� �� ��, ��� ���� ������� ����������
            // �������� ����� Create ����������� � ������ ����� ������� � ���������
        mock.Verify(repository => repository.Create(It.IsAny<DotNetMetric>()),Times.AtMostOnce());
        }
    }
}