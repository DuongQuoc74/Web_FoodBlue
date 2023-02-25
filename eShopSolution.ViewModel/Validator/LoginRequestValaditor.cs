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
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name must not be left blank!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password must not be left blank!")
                .MinimumLength(8).WithMessage("The password must be at least 8 characters!");
        }
    }
}
