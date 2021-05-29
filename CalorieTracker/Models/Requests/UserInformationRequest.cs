using System;
using CalorieTracker.Models.Enums;

namespace CalorieTracker.Models.Requests
{
    public class UserInformationRequest
    {
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Height { get; set; }
        public decimal Weight { get; set; }
        public decimal MinCalories { get; set; }
        public decimal MaxCalories { get; set; }    
    }
}