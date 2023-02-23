using eShopSolution.ViewModel.System.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Validator
{
    public class LoginRequestValaditor : AbstractValidator<LoginRequest>
    {
        public LoginRequestValaditor()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Tài khoản không được để trống!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Mật khẩu Không được để trống!")
                .MinimumLength(8).WithMessage("Mật khẩu phải ít nhất 8 kí tự!");
        }
    }
}
