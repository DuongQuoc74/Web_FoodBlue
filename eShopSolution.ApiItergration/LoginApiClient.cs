using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eShopSolution.ApiItergration
{
    public class LoginApiClient : ILoginApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<string>> AuthenticateAdmin(LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8,"application/json");

            var response = await client.PostAsync("/api/users/authenticateAdmin", httpContent);
            var body= await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);

            return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
                
         

        }
    }
}
