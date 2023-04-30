using Microsoft.AspNetCore.Identity;

namespace DMed_Razor.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }

    }
}