using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CalorieTracker.Models
{
    public class AppRole : IdentityRole
    {
        public DateTimeOffset CreatedAt { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}