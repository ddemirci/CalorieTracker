using System;
using System.Collections.Generic;
using CalorieTracker.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace CalorieTracker.Models
{
    public class AppUser : IdentityUser
    {
        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        public DateTimeOffset EnrolledAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }

        #region NavigationProperties

        public ICollection<AppUserRole> UserRoles { get; set; }
        public ICollection<Meal> Meals { get; set; }
        public UserInformation UserInformation { get; set; }

        #endregion

    }
}