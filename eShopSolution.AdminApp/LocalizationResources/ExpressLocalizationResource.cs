namespace eShopSolution.AdminApp.LocalizationResources
{
    public class ExpressLocalizationResource
    {
       public class User
        {
            public class LoginRequest
            {
                public static string UserName => "User name must not be left blank!";
                public static string PassWord => "Password must not be left blank!";
                public static string PassWordLength => "The password must be at least 8 characters!";
                public static string UserNameApi => "Incorrect account or password!";
                public static string PassWordApi => "This account is not granted access!";
            }
            public class RegisterRequest
            {
                public static string UserName => "User name must not be left blank!";
                public static string UserNameLength => "The user name must be at least 5!";
                public static string PassWord => "Password must not be left blank!";
                public static string PassWordLength => "The password must be at least 8 characters!";
                public static string ConfirmPassWord => "Confirm Password is not match!";
                public static string ConfirmPassWordNull => "Confirm that the password is not blank!";
                public static string FirstName => "First name must not be blank!";
                public static string LastName => "Last name must not be blank!";
                public static string Dob => "Date of birth Must not exceed 100 years old!";
                public static string Email => "Email must not be left blank!";
                public static string EmailFormat => "Email format not match!";
                public static string PhoneNumber => "PhoneNumber must not be left blank!";
                public static string PhoneNumberLength => "The phone number must be at least 10!";
                public static string PhoneNumberFormat => "Please enter a valid number!";


                public static string UserNameApi => "The account already exists!";
                public static string EmailApi => "The email already exists!";
                public static string PhoneNumberApi => "The phone number already exists!";
                public static string AccoutCreate => "Account registration failed!";
                public static string CreateSuccess => "Create user is success!";
            }
            public class Delete
            {
                public static string DeleteSuccess = "Delete is success!";
                public static string DeleteApi = "Can't delete this account!";
            }

            public class UpdateRequest
            {
                public static string FirstName => "First name must not be blank!";
                public static string LastName => "Last name must not be blank!";
                public static string Dob => "Date of birth Must not exceed 100 years old!";
                public static string Email => "Email must not be left blank!";
                public static string EmailFormat => "Email format not match!";
                public static string PhoneNumber => "PhoneNumber must not be left blank!";
                public static string PhoneNumberLength => "The phone number must be at least 10!";
                public static string PhoneNumberFormat => "Please enter a valid number!";

                public static string EmailApi => "The email already exists!";
                public static string PhoneNumberApi => "The phone number already exists!";
                public static string UpdateFail => "User update failed!";
                public static string UpdateSuccsess => "User update is successful!";
            }
        }
    }
}
