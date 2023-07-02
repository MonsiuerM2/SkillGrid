using API.Services;
using AutoMapper;
using DMed_Razor.Data;
using DMed_Razor.DTOs.LMEs;
using DMed_Razor.Entities;
using DMed_Razor.Helpers;
using DMed_Razor.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DMed_Razor.Controllers
{
    public class LecturerController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly EmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly OrganizationHelper _orgHelper;
        private readonly AccountHelper _accountHelper;
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;
        public LecturerController(IMapper mapper, ILogger<AccountController> logger, ITokenService tokenService,
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
        public async Task<IActionResult> RequestModuleAssign(ModuleAssignRequestDto moduleAssignRequestDto)
        {
            if (!await _accountHelper.UserExists(moduleAssignRequestDto.LecturerId))
            {
                return NotFound("There is no user by that ID.");
            }
            {
                if (!await _accountHelper.CheckUserRole(moduleAssignRequestDto.LecturerId, "Lecturer"))
                {
                    return NotFound("There is no lectuer by that ID.");
                }
                AppUser lecturer = await _accountHelper.GetUser(moduleAssignRequestDto.LecturerId);
                List<AppUser> users = await _accountHelper.GetUsersByRoleId(3);

                foreach (AppUser appUser in users)
                {
                    var IsCompletedSuccessfully = await _orgHelper.SendEmail(
                        appUser.Email,
                       "Module Assignment Request",
                       "A lecturer with the name of " + lecturer.Name + " has reuqest to be assigned module for them to teach. Their qualifications/details are: <br>" +
                       "1) Subject: " + moduleAssignRequestDto.Subject + "<br>" +
                       "2) Grade Level: " + moduleAssignRequestDto.GradeLevel + "<br>" +
                       "3) Duration: " + moduleAssignRequestDto.Duration + "<br>" +
                       "4) Preferred Teaching Methodology: " + moduleAssignRequestDto.PreferredTeachingMethodology + "<br>" +
                       "5) Additional Information: " + moduleAssignRequestDto.AdditionalInformation + "<br>" +
                       "6) Specific Requirements: <br>" + string.Join("&#9; <br>  &#9;", moduleAssignRequestDto.SpecificRequirements) + "<br>"
                    );
                }

                return Ok("PLease wait 2-3 business days while the admin team reviews your request.");
            }

        }
    }
}