using System;
using CalorieTracker.Models.Enums;

namespace CalorieTracker.Models
{
    public class UserInformation
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Height { get; set; }
        public decimal Weight { get; set; }
        public decimal MinCalories { get; set; }
        public decimal MaxCalories { get; set; }        
        
        #region NavigationProperties

        public AppUser User { get; set; }

        #endregion
    }
}