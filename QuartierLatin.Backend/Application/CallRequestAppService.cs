using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Models.CallRequest;
using QuartierLatin.Backend.Utils;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuartierLatin.Backend.Config;

namespace QuartierLatin.Backend.Application
{
    public class CallRequestAppService : ICallRequestAppService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public CallRequestAppService(ILoggerFactory loggerFactory, IOptions<CallRequestConfig> config)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(config.Value.RequestUrl)
            };
            _logger = loggerFactory.CreateLogger(nameof(CallRequestAppService));
        }

        public async Task<bool> SendCallRequest(CallRequest requestCall)
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
    }
}
