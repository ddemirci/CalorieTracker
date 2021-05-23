using System.Collections.Generic;
using CalorieTracker.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace CalorieTracker.Models
{
    public class AppUser : IdentityUser
    {
        public Gender Gender { get; set; }
        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}