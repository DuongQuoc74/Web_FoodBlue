using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
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
        private readonly RoleManager<AppRole> _roleManager;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
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

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user.UserName=="admin")
                return new ApiErrorResult<bool>("Can't delete this account!");
             await _userManager.DeleteAsync(user);
           
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<string>> Edit(Guid id, UpdateUserRequest request)
        {
           var email = await _userManager.Users.AnyAsync(x=>x.Id != id && x.Email==request.Email);
            if (email)
                return new ApiErrorResult<string>("The email already exists!");
            var phoneNumber = await _userManager.Users.AnyAsync(x=>x.Id!=id && x.PhoneNumber==request.PhoneNumber);
            if (phoneNumber)
                return new ApiErrorResult<string>("The phone number already exists!");
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Dob=request.Dob;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return new ApiErrorResult<string>("User update failed!");
            return new ApiSuccessResult<string>();
        }

        public async Task<List<RoleVM>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleVM()
            {
                Name = x.Name,
                Description = x.Description,
                Id = x.Id
            }).ToListAsync();
            return roles;
        }

        public async Task<ApiResult<UserVM>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            var data = new UserVM()
            {
                FristName = user.FirstName,
                LastName = user.LastName,
                Dob = user.Dob,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles =roles

            };
            return new ApiSuccessResult<UserVM>(data);
        }

        public async Task<ApiResult<PageResult<UserVM>>> GetAllPagings(PadingRequest request)
        {
            var users = _userManager.Users;
            if( request.KeyWord!=null)
            {
                users = users.Where(x=>x.UserName.Contains(request.KeyWord) ||x.FirstName.Contains(request.KeyWord)||
                x.PhoneNumber.Contains(request.KeyWord)|| x.Email.Contains(request.KeyWord));
            };
            var totalRow = await users.CountAsync();
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
                TotalRecords = totalRow,
                
            };
            return new ApiSuccessResult<PageResult<UserVM>>(pageResult);

        }

        public async Task<ApiResult<string>> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null) return new ApiErrorResult<string>("The account already exists!");
            var email =await _userManager.FindByEmailAsync(request.Email);
            if (email != null) return new ApiErrorResult<string>("The email already exists!");
            var phone = await _userManager.Users.AnyAsync(x=>x.PhoneNumber== request.PhoneNumber);
            if (phone) return new ApiErrorResult<string>("The phone number already exists!");
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
            if (result.Succeeded==false) return new ApiErrorResult<string>("Account registration failed!");
            return new ApiSuccessResult<string>();
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var removeRoleAsign = request.Items.Where(x => x.Selected == false).Select(x => x.Name).ToList();
           foreach(var roleName in removeRoleAsign)
                if( await _userManager.IsInRoleAsync(user, roleName))
                    await _userManager.RemoveFromRoleAsync(user, roleName);
            var addRoleAsign = request.Items.Where(x => x.Selected == true).Select(x => x.Name).ToList();
            foreach (var roleName in addRoleAsign)
                if (await _userManager.IsInRoleAsync(user, roleName)==false)
                    await _userManager.AddToRoleAsync(user, roleName);
            return new ApiSuccessResult<bool>();

        }
    }
}
