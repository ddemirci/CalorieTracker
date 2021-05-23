using Microsoft.AspNetCore.Identity;

namespace CalorieTracker.Models
{
    public class AppUserRole : IdentityUserRole<string>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}