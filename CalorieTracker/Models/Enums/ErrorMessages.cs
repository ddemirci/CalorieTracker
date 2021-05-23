﻿using System.ComponentModel;
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
        
        [Display(Description = "Phone number cannot be empty")]
        PhoneNumberCannotBeEmpty
    }
}