using eShopsolution.Utilities.Constants;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
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

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemContants.BaseAddress]);

            var token = _httpContextAccessor.HttpContext.Session.GetString(SystemContants.Token);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var reponse = await client.DeleteAsync($"/api/users/{id}");
            var body = await reponse.Content.ReadAsStringAsync();
            if(!reponse.IsSuccessStatusCode) 
                return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
            return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);
        }

        public async Task<ApiResult<List<RoleVM>>> GetAllRoles()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemContants.BaseAddress]);
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemContants.Token) ;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var reponse = await client.GetAsync("/api/users/roles");
            var body  = await reponse.Content.ReadAsStringAsync();
            if (!reponse.IsSuccessStatusCode)
                return new ApiErrorResult< List<RoleVM>>(body);

            List<RoleVM> rolesDeserializeObj  = (List<RoleVM>)JsonConvert.DeserializeObject(body, typeof(List<RoleVM>));
            return new ApiSuccessResult<List<RoleVM>>(rolesDeserializeObj); 
        }

        public async Task<ApiResult<UserVM>> GetById(Guid id)
        {
            
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemContants.BaseAddress]);
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemContants.Token) ;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var reponse = await client.GetAsync($"/api/users/{id}");
            var body = await reponse.Content.ReadAsStringAsync();
            if(!reponse.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiErrorResult<UserVM>>(body); 
            return JsonConvert.DeserializeObject<ApiSuccessResult<UserVM>>(body) ;
        }

        public async Task<ApiResult<PageResult<UserVM>>> GetPadingUser(PadingRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemContants.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemContants.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var reponse = await client.GetAsync($"/api/users/pading?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.KeyWord}");
            var body = await reponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ApiResult<PageResult<UserVM>>>(body);
            return users;
        }

        public async Task<ApiResult<string>> Register(RegisterRequest request)
        {
            
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemContants.BaseAddress]);

            var json = JsonConvert.SerializeObject(request);
            var httpcontent = new StringContent(json, Encoding.UTF8,"application/json");

            var reponse = await client.PostAsync("/api/users/register",httpcontent);
            var body = await reponse.Content.ReadAsStringAsync();
            if (!reponse.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
        }

        public async Task<ApiResult<string>> Update(Guid id, UpdateUserRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemContants.BaseAddress]);
            var session = _httpContextAccessor.HttpContext.Session.GetString(SystemContants.Token);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);
            
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var reponse = await client.PutAsync($"/api/users/{id}", httpContent);
            var body = await reponse.Content.ReadAsStringAsync();
            if (!reponse.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(body);
            return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(body);
        }
    }
}
