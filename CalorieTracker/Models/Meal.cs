using System;
using System.Collections.Generic;

namespace CalorieTracker.Models
{
    public class Meal
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public DateTimeOffset EatenAt { get; set; } = DateTimeOffset.Now;
        
        #region NavigationProperties

        public AppUser User { get; set; } 
        public ICollection<MealFood> MealFoods { get; set; }
        
        #endregion
    }
}