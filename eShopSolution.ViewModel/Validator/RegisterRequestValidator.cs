using eShopSolution.ViewModel.System.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Validator
{
    public class RegisterRequestValidator :AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator() 
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name must not be left blank!")
                .MinimumLength(5).WithMessage("The user name must be at least 5!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password must not be left blank!").
                MinimumLength(8).WithMessage("The password must be at least 8 characters!");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.ComfirmPassword == null)
                {
                    context.AddFailure("Confirm that the password is not blank!");
                }
                else if (request.Password != request.ComfirmPassword)
                {
                    context.AddFailure("Confirm Password is not match!");
                }
            });
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name must not be blank!");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name must not be blank!");

            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Date of birth Must not exceed 100 years old!");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email must not be left blank!")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email format not match!");

            RuleFor(x => x.PhoneNumber).NotEmpty()
                .MinimumLength(10).WithMessage("The phone number must be at least 10!")
                .Must(BeAValidNumber).WithMessage("Please enter a valid number!");
        }
        private bool BeAValidNumber(string number)
        {
            return double.TryParse(number, out double result);
        }
    }
}
