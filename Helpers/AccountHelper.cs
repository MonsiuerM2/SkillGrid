using DMed_Razor.Entities;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DMed_Razor.Helpers
{
    public class AccountHelper
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AccountHelper(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> UserExists(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> UserExists(int userId, string userRole)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return false;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.Contains(userRole);
        }
        public async Task<bool> CheckUserRole(int userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _roleManager.FindByNameAsync(roleName);
            var isInRole = await _userManager.IsInRoleAsync(user, roleName);

            return isInRole;
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
        public async Task<bool> EmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
        public async Task<List<AppUser>> GetUsersByRoleId(int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            return usersInRole.ToList();
        }
        public async Task<AppUser> GetUser(int userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }
        public byte[] CreateSalt()
        {
            var buffer = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
                var randomNumber = BitConverter.ToInt32(buffer, 0);
            }
            return buffer;
        }
        public byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; // four cores
            argon2.Iterations = 4;
            argon2.MemorySize = 1024 * 1024; // 1 GB

            return argon2.GetBytes(16);
        }
        public bool VerifyHash(string password, byte[] salt, byte[] hash)
        {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash);
        }
    }


}
