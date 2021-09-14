using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuartierLatin.Backend.Application.ApplicationCore.Interfaces.Services;
using QuartierLatin.Backend.Application.ApplicationCore.Models.CallRequest;
using QuartierLatin.Backend.Config;
using QuartierLatin.Backend.Utils;

namespace QuartierLatin.Backend.Services
{
    public class CallRequestAppService : ICallRequestAppService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public CallRequestAppService(ILoggerFactory loggerFactory, IOptions<CallRequestConfig> config)
        {
            _logger = loggerFactory.CreateLogger(nameof(CallRequestAppService));
            try
            {
                _httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(config.Value.RequestUrl)
                };
            }
            catch (Exception e)
            {
                _logger.LogCritical("Failed init http client: "+ e.ToString());
            }
        }

        public async Task<bool> SendCallRequest(CallRequest requestCall)
        {
            try
            {
                var content = GetCallRequestContent.GetContent(
                    requestCall.Url + " " + requestCall.FirstName + " " + requestCall.LastName,
                    requestCall.FirstName, requestCall.LastName, requestCall.Phone, requestCall.Email,
                    requestCall.Comment + "\r\n" + requestCall.Comment);

                var req = await content.ReadAsStringAsync();

                _logger.LogInformation("Sending " + req);
                var response = await _httpClient.PostAsync(_httpClient.BaseAddress, content);
                _logger.LogInformation("Got response " + await response.Content.ReadAsStringAsync());

                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                _logger.LogCritical("Error when sending call request: " + e.ToString());
                return false;
            }
        }
    }
}
