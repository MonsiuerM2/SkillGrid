using DMed_Razor.Entities;

namespace DMed_Razor.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);

        string CreateEmailVerificationToken(string email);
    }
}