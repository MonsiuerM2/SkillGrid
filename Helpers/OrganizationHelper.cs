using API.Services;
using DMed_Razor.Data;
using Microsoft.EntityFrameworkCore;

namespace DMed_Razor.Helpers
{
    public class OrganizationHelper
    {
        private readonly DataContext _context;
        private readonly EmailSender _emailSender;

        public OrganizationHelper(DataContext context, EmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public async Task<bool> OrgIdExists(int OrgId)
        {
            return await _context.Organizations.AnyAsync(x => x.OrgId == OrgId);
        }

        public async Task<bool> OrgNameExists(string orgName)
        {
            return await _context.Organizations.AnyAsync(x => x.OrgName == orgName.ToLower());
        }

        public async Task<bool> SendEmail(string userEmail, string subject, string message)
        {
            bool sentToken = await _emailSender.SendEmailAsync(userEmail.ToLower(), subject, message);

            return sentToken;
        }
    }
}
