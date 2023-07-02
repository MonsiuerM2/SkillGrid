using DMed_Razor.Entities;
using DMed_Razor.Interfaces;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DMed_Razor.Data
{
    public class Seed
    {
        //private readonly ITokenService _tokenService;

        //public Seed(ITokenService tokenService)
        //{
        //    _tokenService = tokenService;
        //}

        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, ITokenService _tokenService)
        {
            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Student"},
                new AppRole{Name = "Lecturer"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
                new AppRole{Name = "Organization"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var test1 = new AppUser
            {
                UserName = "test1",
                Name = "test1",
                City = "Karachi",
                Gender = "Male",
                CNIC = "4210135750433",
                Email = "syedmurtazas19@gmail.com",
                EmailConfirmed = true
            };
            var test2 = new AppUser
            {
                UserName = "test2",
                Name = "test2",
                City = "Karachi",
                Gender = "Male",
                CNIC = "4210135750433",
                Email = "syedmurtazas19@gmail.com",
                EmailConfirmed = true
            };
            var test3 = new AppUser
            {
                UserName = "test3",
                Name = "test3",
                City = "Karachi",
                Gender = "Male",
                CNIC = "4210135750433",
                Email = "syedmurtazas19@gmail.com",
                EmailConfirmed = true
            };
            var admin = new AppUser
            {
                UserName = "admin",
                Name = "admin",
                City = "Karachi",
                Gender = "Male",
                CNIC = "4210135750433",
                Email = "syedmurtazas19@gmail.com",
                EmailConfirmed = true
            };

            string text = test3.Name + test3.Email + test3.CNIC + test3.DOB + test3.City + test3.Gender;

            var salt = CreateSalt();

            var UnqiueHash = HashPassword(text, salt);

            test1.UnqiueHash = UnqiueHash;
            test2.UnqiueHash = UnqiueHash;
            test3.UnqiueHash = UnqiueHash;
            admin.UnqiueHash = UnqiueHash;


            test1.EmailVerificationToken = _tokenService.CreateEmailVerificationToken(test1.UserName + test1.Email);
            test2.EmailVerificationToken = test1.EmailVerificationToken;
            test3.EmailVerificationToken = test1.EmailVerificationToken;
            admin.EmailVerificationToken = test1.EmailVerificationToken;

            await userManager.CreateAsync(test1, "Pa$$w0rd");
            await userManager.AddToRolesAsync(test1, new[] { "Student" });
            await userManager.CreateAsync(test2, "Pa$$w0rd");
            await userManager.AddToRolesAsync(test2, new[] { "Student" });
            await userManager.CreateAsync(test3, "Pa$$w0rd");
            await userManager.AddToRolesAsync(test3, new[] { "Student" });
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }

        private static byte[] CreateSalt()
        {
            var buffer = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
                var randomNumber = BitConverter.ToInt32(buffer, 0);
            }
            return buffer;
        }
        private static byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; // four cores
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 1024; // 1 GB

            return argon2.GetBytes(16);
        }
    }
}
