using QuartierLatin.Backend.Application.Interfaces;
using QuartierLatin.Backend.Models.CallRequest;
using QuartierLatin.Backend.Utils;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using QuartierLatin.Backend.Config;

namespace QuartierLatin.Backend.Application
{
    public class CallRequestAppService : ICallRequestAppService
    {
        private readonly HttpClient _httpClient;
        public CallRequestAppService(IOptions<CallRequestConfig> config)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(config.Value.RequestUrl)
            };
        }

        public async Task<bool> SendCallRequest(CallRequest requestCall)
        {
            var content = GetCallRequestContent.GetContent(
                requestCall.Url + " " + requestCall.FirstName + " " + requestCall.LastName,
                requestCall.FirstName, requestCall.LastName, requestCall.Phone, requestCall.Email,
                requestCall.Comment + "\r\n" + requestCall.Comment);

            var response = await _httpClient.PostAsync(_httpClient.BaseAddress, content);

            return response.IsSuccessStatusCode;
        }
    }
}
