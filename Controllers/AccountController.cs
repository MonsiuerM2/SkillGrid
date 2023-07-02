using API.Services;
using AutoMapper;
using DMed_Razor.DTOs;
using DMed_Razor.DTOs.AMEs;
using DMed_Razor.Entities;
using DMed_Razor.Helpers;
using DMed_Razor.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMed_Razor.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly EmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly AccountHelper _accountHelper;
        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
                                IMapper mapper, ILogger<AccountController> logger,
                                ITokenService tokenService, EmailSender emailSender)
        {
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _tokenService = tokenService;
            _accountHelper = new AccountHelper(userManager, roleManager);

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await _accountHelper.UsernameExists(registerDto.Username))
            {
                return BadRequest("Username is taken, please try another one.");
            }
            /*if (registerDto.IsLecturer && registerDto.IsOrganization)
            {
                return BadRequest("A user having an organization role can only have an organization role.");
            }*/
            var endDate = DateTime.Parse(registerDto.DOB);

            var user = _mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();
            user.DOB = DateTime.Parse(registerDto.DOB);

            user.EmailVerificationToken = _tokenService.CreateEmailVerificationToken(user.UserName + user.Email);

            if (user.DOB > DateTime.Now)
            {
                return BadRequest("The entered Date of Birth is in the future.");
            }

            //Unique Hash Generation
            string text = user.Name + user.Email + user.CNIC + user.DOB + user.City + user.Gender;
            var salt = _accountHelper.CreateSalt();
            var UnqiueHash = _accountHelper.HashPassword(text, salt);
            user.UnqiueHash = UnqiueHash;

            Console.WriteLine(UnqiueHash + " <-----------------------------------------------------------");

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            String role = "student";
            if (registerDto.IsLecturer)
            {
                role = "lecturer";
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var IsCompletedSuccessfully = await SendEmail(user.Email, user.EmailVerificationToken,
                "Verify Your Email", "Click the link bellow to verify your email.");

            return Ok("Account successfully registered");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Please enter a valid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) return Unauthorized("Invalid Password");

            if (user.EmailConfirmed == false) return Unauthorized("Please verify your email first");

            return new UserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user)
            };
        }

        [HttpPost("resend-email")]
        public async Task<IActionResult> ResendVerificationEmail(EmailDto emailDto)
        {
            Console.WriteLine(emailDto.Username.ToLower() + " " + emailDto.Email);
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == emailDto.Username.ToLower());

            if (user == null) return Unauthorized("Please enter a valid username");

            user.EmailVerificationToken = _tokenService.CreateEmailVerificationToken(user.UserName + user.Email);
            var result = await _userManager.UpdateAsync(user);

            if (user == null) return Unauthorized("Please enter a valid username");
            if (user.EmailConfirmed) return BadRequest("Your email is already verified");

            var IsCompletedSuccessfully = await SendEmail(emailDto.Email, user.EmailVerificationToken,
                    "Verify Your Email", "Click the link bellow to verify your email.");

            if (IsCompletedSuccessfully == false)
            {
                return BadRequest("Email could not be sent, please try again.");
            }
            return Ok("Email sent successfully");
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            //_logger.LogInformation(" %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            //Console.WriteLine(token + " %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.EmailVerificationToken == token);
            if (user == null)
            {
                return BadRequest("Email verification failed");
            }

            user.EmailConfirmed = true;
            user.EmailVerificationToken = "";

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("Email verified successfully");
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto rpDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == rpDto.Username.ToLower());

            if (user == null) return Unauthorized("Please enter a valid username");

            //Console.WriteLine("\n----------------------------------------\n" + rpDto.token + "\n----------------------------------------");
            string token = rpDto.token.Replace(" ", "+");


            Console.WriteLine("><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><");
            var result = await _userManager.ResetPasswordAsync(user, token, rpDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            if (user.EmailConfirmed) user.EmailVerificationToken = "";

            await _userManager.UpdateAsync(user);

            return Ok("Password reset successfully");
        }
        [HttpPost("reset-password-email")]
        public async Task<IActionResult> ResetPasswordEmail(UserrnameDto userrnameDto)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == userrnameDto.Username.ToLower());

            if (user == null) return Unauthorized("Please enter a valid username");
            if (!user.EmailConfirmed) return BadRequest("Email is not verified");
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            //Console.WriteLine("\n----------------------------------------\n" + passwordResetToken + "\n----------------------------------------");

            user.EmailVerificationToken = passwordResetToken;

            await _userManager.UpdateAsync(user);

            var IsCompletedSuccessfully = await SendResetEmail(user.Email, passwordResetToken,
                    "Reset Your Password", "Click the link bellow to reset your password.");

            return Ok("Email sent successfully");
        }
        private async Task<bool> SendEmail(string userEmail, string EmailVerificationToken, string subject, string message)
        {
            var verificationUrl = Url.Action("VerifyEmail", "Account", new { token = EmailVerificationToken }, Request.Scheme);
            bool sentToken = await _emailSender.SendEmailAsync(userEmail.ToLower(), subject, message + $"\nThis link will expire in one hour.\n<a href='{verificationUrl}'>Click Here</a> ");

            return sentToken;
        }
        private async Task<bool> SendResetEmail(string userEmail, string EmailVerificationToken, string subject, string message)
        {
            //Console.WriteLine(EmailVerificationToken + "------------------------------------<<<<<<<<<<<<<<<<");
            var verificationUrl = $"https://localhost:4200/reset-password?token={EmailVerificationToken}";

            bool sentToken = await _emailSender.SendEmailAsync(userEmail.ToLower(), subject, message + $"\nThis link will expire in one hour.\n<a href='{verificationUrl}'>here</a> ");

            return sentToken;
        }

    }
}