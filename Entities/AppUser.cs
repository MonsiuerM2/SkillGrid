using Microsoft.AspNetCore.Identity;

namespace DMed_Razor.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        new public string Email { get; set; }
        public string CNIC { get; set; }
        public DateTime DOB { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public byte[] UnqiueHash { get; set; }
        new public bool EmailConfirmed { get; set; }
        public string EmailVerificationToken { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}