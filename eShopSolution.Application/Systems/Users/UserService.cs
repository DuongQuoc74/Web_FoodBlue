using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eShopSolution.Application.Systems.Users
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<ApiResult<string>> AuthenticateAdmin(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ApiErrorResult<string>("Incorrect account or password!");
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded) return new ApiErrorResult<string>("Incorrect account or password!");
            var role = await _userManager.GetRolesAsync(user);
            if (role.Count == 0) return new ApiErrorResult<string>("This account is not granted access!");
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Role, string.Join(";", role)),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                _configuration["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            return new ApiSuccessResult<string>( new JwtSecurityTokenHandler().WriteToken(token) );
        }

        public async Task<ApiResult<PageResult<UserVM>>> GetPadingRequest(PadingRequest request)
        {
            var users = _userManager.Users;
            if( request.KeyWord!=null)
            {
                users = users.Where(x=>x.UserName.Contains(request.KeyWord) ||
                x.PhoneNumber.Contains(request.KeyWord)|| x.Email.Contains(request.KeyWord));
            };
            var data = await users.Skip((request.PageIndex-1)*request.PageSize).Take(request.PageSize)
                .Select(x=> new UserVM()
                {
                    Id=x.Id,
                    UserName=x.UserName,
                    FristName  = x.FirstName,
                    LastName=x.LastName,
                    PhoneNumber=x.PhoneNumber,
                    Email=x.Email,
                    Dob=x.Dob,
                }).ToListAsync();

            var pageResult = new PageResult<UserVM>()
            {
                Items = data,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            return new ApiSuccessResult<PageResult<UserVM>>(pageResult);

        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null) return new ApiErrorResult<bool>("Tài khoản đã tồn tại");
            var email =await _userManager.FindByEmailAsync(request.Email);
            if (email != null) return new ApiErrorResult<bool>("Email đã tồn tại");
            var phone = await _userManager.Users.AnyAsync(x=>x.PhoneNumber== request.PhoneNumber);
            if (phone == true) return new ApiErrorResult<bool>("Số điện thoại đã tồn tại");
            if(request.Password!=request.ComfirmPassword) return new ApiErrorResult<bool>("Mật khẩu không khớp!");
            user = new AppUser()
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Dob = request.Dob

            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded==false) return new ApiErrorResult<bool>("Đăng kí tài khoản không thành công");
            return new ApiSuccessResult<bool>();
        }
    }
}
