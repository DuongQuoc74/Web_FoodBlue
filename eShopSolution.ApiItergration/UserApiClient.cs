using eShopsolution.Utilities.Constants;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiItergration
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        public UserApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) 
        {
            _configuration= configuration;
            _httpClientFactory= httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<PageResult<UserVM>>> GetPadingUser(PadingRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemContants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemContants.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var reponse = await client.GetAsync($"api/users/pading?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.KeyWord}");
            var body = await reponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ApiResult<PageResult<UserVM>>>(body);
            return users;
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemContants.BaseAddress]);

            var json = JsonConvert.SerializeObject(request);
            var httpcontent = new StringContent(json, Encoding.UTF8,"application/json");

            var reponse = await client.PostAsync("api/users/register",httpcontent);
            var body = await reponse.Content.ReadAsStringAsync();
            if (!reponse.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
            return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body); 
        }
    }
}
