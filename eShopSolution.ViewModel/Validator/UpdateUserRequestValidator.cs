using eShopSolution.ViewModel.System.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Validator
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator() {
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
