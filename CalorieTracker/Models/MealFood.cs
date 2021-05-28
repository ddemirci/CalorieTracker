using System;

namespace CalorieTracker.Models
{
    public class MealFood
    {
        public Guid Id { get; set; }
        public Guid MealId { get; set; }
        public decimal Amount { get; set; }
        public decimal Calories { get; set; }
        public decimal TotalFat { get; set; }
        public decimal SaturatedFat { get; set; }
        public decimal Cholesterol { get; set; }
        public decimal Sodium { get; set; }
        public decimal Carbohydrate { get; set; }
        public decimal DietaryFiber { get; set; }
        public decimal Sugars { get; set; }
        public decimal Protein { get; set; }
        public decimal Potassium { get; set; }
        
        #region Navigation Properties
        
        public Meal Meal { get; set; }
        
        #endregion
    }
}
