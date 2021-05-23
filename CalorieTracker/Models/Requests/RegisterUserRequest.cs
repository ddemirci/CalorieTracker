﻿using System.ComponentModel.DataAnnotations;
using CalorieTracker.Models.Enums;

namespace CalorieTracker.Models.Requests
{
    public class RegisterUserRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
    }
}