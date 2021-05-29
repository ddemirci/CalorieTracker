using System.ComponentModel.DataAnnotations;

namespace CalorieTracker.Models.Enums
{
    public enum ErrorMessages : int
    {
        [Display(Description = "User name cannot be empty")]
        UserNameCannotBeEmpty,
        
        [Display(Description = "User name length should be at least {0} and at most {1}")]
        UserNameLengthMustBetweenXAndY,
        
        [Display(Description = "Email has already been taken")]
        UserNameIsAlreadyTaken,
        
        [Display(Description = "User name cannot start with digits")]
        UserNameCannotStartWithDigit,

        [Display(Description = "Email address cannot be empty")]
        EmailCannotBeEmpty,
        
        [Display(Description = "Email address is in invalid format")]
        EmailFormatIsInvalid,
        
        [Display(Description = "Email has already been using")]
        EmailIsAlreadyTaken,

        [Display(Description = "First name cannot be empty")]
        FirstNameCannotBeEmpty,

        [Display(Description = "Last name cannot be empty")]
        LastNameCannotBeEmpty,

        [Display(Description = "User name cannot be empty")]
        PasswordCannotBeEmpty,

        [Display(Description = "Passwords should match")]
        PasswordsShouldMatch,
        
        [Display(Description = "Password length must be at least {0}")]        
        PasswordLengthMustBeAtLeastX,
        
        [Display(Description = "Password should not contain username")]        
        PasswordShouldNotContainUserName,
        
        [Display(Description = "Phone number cannot be empty")]
        PhoneNumberCannotBeEmpty,
        
        [Display(Description = "Role name cannot be empty")]        
        RoleNameCannotBeEmpty,
        
        [Display(Description = "Specified role could not be found")]        
        RoleNotFound,
        
        [Display(Description = "Specified role has already been created")]        
        RoleAlreadyCreated,

        [Display(Description = "You entered a wrong password.")]
        WrongPassword,
        
        [Display(Description = "You have entered wrong username/email or password")]
        LoginInformationWrong,

        [Display(Description = "Your account is suspended until {0}")]
        AccountSuspendedUntilX,

        [Display(Description = "Please enter a valid height")]
        HeightIsNotValid,

        [Display(Description = "Please enter a valid weight")]
        WeightIsNotValid,
        
        [Display(Description = "Please enter a valid gender")]
        GenderIsNotValid,
        
        [Display(Description = "Please enter a valid min calories amount")]
        MinCaloriesIsNotValid,
        
        [Display(Description = "Please enter a valid max calories amount")]
        MaxCaloriesIsNotValid,
        
        [Display(Description = "Please enter a valid birth date")]
        DateOfBirthIsNotValid,
    }
}