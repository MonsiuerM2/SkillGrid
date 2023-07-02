using API.Services;
using AutoMapper;
using DMed_Razor.Data;
using DMed_Razor.DTOs.OMEs;
using DMed_Razor.Entities;
using DMed_Razor.Helpers;
using DMed_Razor.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMed_Razor.Controllers
{
    public class OrganizationController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly EmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly OrganizationHelper _orgHelper;
        private readonly AccountHelper _accountHelper;
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;

        public OrganizationController(IMapper mapper, ILogger<AccountController> logger, ITokenService tokenService,
                                        EmailSender emailSender,
                                        RoleManager<AppRole> roleManager, UserManager<AppUser> userManager,
                                        DataContext context)
        {
            _mapper = mapper;
            _logger = logger;
            _emailSender = emailSender;
            _tokenService = tokenService;
            _userManager = userManager;
            _accountHelper = new AccountHelper(userManager, roleManager);
            _orgHelper = new OrganizationHelper(context, emailSender);
            _context = context;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestOrgAddition(OrgRegisterDto orgRegisterDto)
        {

            if (await _orgHelper.OrgNameExists(orgRegisterDto.OrgName))
            {
                return BadRequest("That name is already taken, please try another one.");
            }
            var endDate = DateTime.Parse(orgRegisterDto.DOB);

            if (endDate > DateTime.Now)
            {
                return BadRequest("The entered Date of Birth is in the future.");
            }

            List<AppUser> users = await _accountHelper.GetUsersByRoleId(3);

            foreach (AppUser appUser in users)
            {
                var IsCompletedSuccessfully = await _orgHelper.SendEmail(
                    appUser.Email,
                    "Organization Addition Request",
                    "Organization Details: <br><br>" +
                    "Name: " + orgRegisterDto.OrgName + "<br>" +
                    "Services: " + orgRegisterDto.Services + "<br>" +
                    "Website Url: " + orgRegisterDto.WebsiteUrl + "<br>" +
                    "Work Type: " + orgRegisterDto.WorkType + "<br><br>" +

                    "Point Person Details: <br><br>" +
                    "Name: " + orgRegisterDto.Name + "<br>" +
                    "CNIC: " + orgRegisterDto.CNIC + "<br>" +
                    "DOB: " + orgRegisterDto.DOB + "<br>" +
                    "Gender: " + orgRegisterDto.Gender + "<br><br>"

                );

            }

            return Ok("PLease wait 2-3 business days while the admin team reviews your request.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-org")]
        public async Task<IActionResult> AddOrganization(OrgAddDto orgAddDto)
        {
            var org = _mapper.Map<Organization>(orgAddDto);

            var endDate = DateTime.Parse(orgAddDto.DOB);

            if (endDate > DateTime.Now)
            {
                return BadRequest("The entered Date of Birth is in the future.");
            }

            if (await _accountHelper.UsernameExists(orgAddDto.Username))
            {
                return BadRequest("Username is taken, please try another one.");
            }

            var user = _mapper.Map<AppUser>(orgAddDto);
            user.UserName = orgAddDto.Username.ToLower();
            user.DOB = endDate;

            user.EmailVerificationToken = _tokenService.CreateEmailVerificationToken(user.UserName + user.Email);

            //Unique Hash Generation
            string text = user.Name + user.Email + user.CNIC + user.DOB + user.City + user.Gender;
            var salt = _accountHelper.CreateSalt();
            var UnqiueHash = _accountHelper.HashPassword(text, salt);
            user.UnqiueHash = UnqiueHash;
            

            Console.WriteLine(UnqiueHash + " <-----------------------------------------------------------");

            var result = await _userManager.CreateAsync(user, orgAddDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Organization");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            await _context.SaveChangesAsync();
            org.UserId = user.Id;
            await _context.Organizations.AddAsync(org);
            await _context.SaveChangesAsync();

            await _orgHelper.SendEmail(user.Email,
                        "Organization Account Made Successfully", "This is your dashboard credentials:" + "<br><br>" +
                                                         "Username: " + orgAddDto.Username + "<br>" +
                                                         "Password: " + orgAddDto.Password + "<br>" +
                                                         "Also please verify your email.");

            var IsCompletedSuccessfully = await SendEmail(user.Email, user.EmailVerificationToken,
                "Verify Your Email", "Click the link bellow to verify your email.");



            return Ok("Account successfully registered");
        }

        private async Task<bool> SendEmail(string userEmail, string EmailVerificationToken, string subject, string message)
        {
            var verificationUrl = Url.Action("VerifyEmail", "Account", new { token = EmailVerificationToken }, Request.Scheme);
            bool sentToken = await _emailSender.SendEmailAsync(userEmail.ToLower(), subject, message + $"\nThis link will expire in one hour.\n<a href='{verificationUrl}'>Click Here</a> ");

            return sentToken;
        }
    }

}