using MetricsManager.Responses;
using MetricsManager.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using NLog;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        AllRamMetricsApiResponse GetAllRamMetrics(GetAllRamMetricsApiRequest request);
        AllHddMetricsApiResponse GetAllHddMetrics(GetAllHddMetricsApiRequest request);
        DonNetMetricsApiResponse GetDonNetMetrics(DonNetHeapMetrisApiRequest request);
        AllCpuMetricsApiResponse GetCpuMetrics(GetAllCpuMetricsApiRequest request);
    }   
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        public MetricsAgentClient(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        public AllHddMetricsApiResponse
        GetAllHddMetrics(GetAllHddMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
            $"{request.ClientBaseAddress}/api/hddmetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httprequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return
                JsonSerializer.DeserializeAsync<AllHddMetricsApiResponse>(responseStream).Result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }
        public AllRamMetricsApiResponse
        GetAllRamMetrics(GetAllRamMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
            $"{request.ClientBaseAddress}/api/Rammetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httprequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return
                JsonSerializer.DeserializeAsync<AllRamMetricsApiResponse>(responseStream).Result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }
        public AllCpuMetricsApiResponse
         GetCpuMetrics(GetAllCpuMetricsApiRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
            $"{request.ClientBaseAddress}/api/Cpumetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httprequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return
                JsonSerializer.DeserializeAsync<AllCpuMetricsApiResponse>(responseStream).Result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }
        public DonNetMetricsApiResponse
        GetDonNetMetrics (DonNetHeapMetrisApiRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
            $"{request.ClientBaseAddress}/api/DonNetmetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httprequest).Result;
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return
                JsonSerializer.DeserializeAsync<AllDonNetMetricsApiResponse>(responseStream).Result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }
        return null;
}
}




