using API.Services;
using AutoMapper;
using DMed_Razor.Data;
using DMed_Razor.DTOs.SMEs;
using DMed_Razor.Entities;
using DMed_Razor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMed_Razor.Controllers
{
    public class SessionController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly EmailSender _emailSender;
        private readonly IMapper _mapper;
        private readonly OrganizationHelper _orgHelper;
        private readonly AccountHelper _accountHelper;
        private readonly CourseModuleHelper _cmHelper;

        public SessionController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
                                        IMapper mapper, ILogger<AccountController> logger,
                                        EmailSender emailSender,
                                        DataContext context)
        {
            _mapper = mapper;
            _emailSender = emailSender;
            _cmHelper = new CourseModuleHelper(context);
            _orgHelper = new OrganizationHelper(context, emailSender);
            _accountHelper = new AccountHelper(userManager, roleManager);
            _context = context;
        }

        [HttpGet()]
        public async Task<List<Session>> GetSessions(SessionGetDto sessionGetDto)
        {
            IQueryable<Session> query = _context.Sessions;

            if (sessionGetDto.IncludeCourse)
            {
                query = query.Include(s => s.Course);
            }

            if (sessionGetDto.IncludeOrg)
            {
                query = query.Include(s => s.Organization);
            }

            List<Session> sessions = await query.ToListAsync();

            return sessions;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestSessionAddition(SessionAddDto sessionAddDto)
        {
            ActionResult validationResult = await ValidateSessionAddDto(sessionAddDto);
            if (validationResult != null)
            {
                return validationResult;
            }

            List<string> modulesList = new List<string>();
            string courseName = "";
            Course course = new Course();

            if (sessionAddDto.MakingOwnCourse == true)
            {
                courseName = sessionAddDto.CourseName;

                if (sessionAddDto.ModulesIdList?.Distinct().Count() != sessionAddDto.ModulesIdList?.Count())
                {
                    return BadRequest("There cannot be duplicate modules in the modules list.");
                }

                modulesList = await _context.Modules
                    .Where(m => sessionAddDto.ModulesIdList != null && sessionAddDto.ModulesIdList.Contains(m.ModuleId))
                    .Select(m => m.Name)
                    .ToListAsync();

                if (sessionAddDto.ModulesIdList != null && modulesList.Count() != sessionAddDto.ModulesIdList.Count())
                {
                    return BadRequest("A course's modules should be modules that exist.");
                }
            }

            else
            {
                course = await _context.Courses.SingleOrDefaultAsync(o => o.CourseId == sessionAddDto.CourseId);
                modulesList = await _cmHelper.GetModulesNamesAsync(course.CourseId);
            }

            var orgName = (await _context.Organizations.SingleOrDefaultAsync(o => o.OrgId == sessionAddDto.OrgId)).OrgName;

            List<AppUser> users = await _accountHelper.GetUsersByRoleId(3);

            string text =
                !sessionAddDto.MakingOwnCourse ?
                    orgName + " has selected a premade course <br>" +
                    "Session Details: <br><br>" +
                    "Organization ID: " + sessionAddDto.OrgId + "<br>" +
                    "Organization Name: " + orgName + "<br>" +
                    "Start Date: " + sessionAddDto.StartDate + "<br>" +
                    "End Date: " + sessionAddDto.EndDate + "<br>" +
                    "Maximum Students: " + sessionAddDto.NumStudents + "<br>" +
                    "Course ID: " + sessionAddDto.CourseId + "<br>" +
                    "Course Name: " + course.Name + "<br>" +
                    "Modules: " + string.Join(", ", modulesList)
                    :
                    orgName + " has selected to make a custom course <br>" +
                    "Session Details: <br><br>" +
                    "Organization ID: " + sessionAddDto.OrgId + "<br>" +
                    "Organization Name: " + orgName + "<br>" +
                    "Start Date: " + sessionAddDto.StartDate + "<br>" +
                    "End Date: " + sessionAddDto.EndDate + "<br>" +
                    "Maximum Students: " + sessionAddDto.NumStudents + "<br>" +
                    "Course Name: " + sessionAddDto.CourseName + "<br>" +
                    "Modules: " + string.Join(", ", modulesList);


            foreach (AppUser appUser in users)
            {
                var IsCompletedSuccessfully = await _orgHelper.SendEmail(appUser.Email, "Course Addition Request", text);
            }

            return Ok("PLease wait 2-3 business days while the admin team reviews your request.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add")]
        public async Task<IActionResult> SessionAddition(SessionAddDto sessionAddDto)
        {
            ActionResult validationResult = await ValidateSessionAddDto(sessionAddDto);
            if (validationResult != null)
            {
                return validationResult;
            }

            Organization org = (await _context.Organizations.SingleOrDefaultAsync(o => o.OrgId == sessionAddDto.OrgId));
            Course course = (await _context.Courses.SingleOrDefaultAsync(o => o.CourseId == sessionAddDto.CourseId));
            string orgEmail = (await _accountHelper.GetUser(org.UserId)).Email;

            Session session = _mapper.Map<Session>(sessionAddDto);

            session.Course = course;
            session.Organization = org;

            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();

            string text =
                    "Session Details: <br><br>" +
                    "Organization ID: " + sessionAddDto.OrgId + "<br>" +
                    "Organization Name: " + org.OrgName + "<br>" +
                    "Start Date: " + sessionAddDto.StartDate + "<br>" +
                    "End Date: " + sessionAddDto.EndDate + "<br>" +
                    "Maximum Students: " + sessionAddDto.NumStudents + "<br>" +
                    "Course Name: " + sessionAddDto.CourseName + "<br>" +
                    "Modules: " + string.Join(", ", await _cmHelper.GetModulesNamesAsync(course.CourseId));

            if (sessionAddDto.MakingOwnCourse)
            {
                await _orgHelper.SendEmail(orgEmail, "Course & Session Added", text);
            }
            else
                await _orgHelper.SendEmail(orgEmail, "Session Added of Premade Course", text);

            return Ok("Session Added Succesfully");
        }

        [Authorize(Roles = "Student")]
        [HttpPost("reg")]
        public async Task<IActionResult> RegisterSession(SessionRegisterDto sessionRegisterDto)
        {
            AppUser student = await _accountHelper.GetUser(sessionRegisterDto.StudentId);
            if (student == null)
                return NotFound("No Student found with that ID");

            Session session = await _context.Sessions.SingleOrDefaultAsync(s => s.SessionId == sessionRegisterDto.SessionId);
            if (session == null)
                return NotFound("No Session found with that ID");

            bool isRegistered = await _context.SessionRegistrations
                .AnyAsync(sr => sr.SessionId == sessionRegisterDto.SessionId && sr.StudentId == sessionRegisterDto.StudentId);

            if (isRegistered)
                return BadRequest("You have already registered for that Session");

            SessionRegistration sessionRegistration = new SessionRegistration
            {
                StudentId = student.Id,
                SessionId = session.SessionId,
                Student = student,
                Session = session
            };

            await _context.SessionRegistrations.AddAsync(sessionRegistration);
            await _context.SaveChangesAsync();

            return Ok("Successfully registered for session");
        }

        [HttpGet("get-reg")]
        public async Task<List<SessionRegistration>> GetSessionRegistrations(SessionGetRegsDto sessionGetRegsDto)
        {
            IQueryable<SessionRegistration> query = _context.SessionRegistrations;

            if (sessionGetRegsDto.IncludeStudent)
            {
                query = query.Include(s => s.Student);
            }

            if (sessionGetRegsDto.IncludeSession)
            {
                query = query.Include(s => s.Session);
            }

            List<SessionRegistration> sessionRegistrations = await query.ToListAsync();

            return sessionRegistrations;
        }
        private async Task<ActionResult?> ValidateSessionAddDto(SessionAddDto sessionAddDto)
        {
            if (!(await _orgHelper.OrgIdExists(sessionAddDto.OrgId)))
            {
                return NotFound("No Organization with that ID exists");
            }

            if (!(await _cmHelper.CourseExists(sessionAddDto.CourseId)) && !sessionAddDto.MakingOwnCourse)
            {
                return NotFound("No Course with that ID exists");
            }
            if (sessionAddDto.MakingOwnCourse && (await _cmHelper.CourseExists(sessionAddDto.CourseName)))
            {
                return BadRequest("Course with that Name already exists");
            }
            if (sessionAddDto.MakingOwnCourse && (sessionAddDto.CourseName == ""))
            {
                return BadRequest("Please enter a name for the course");
            }
            return null;
        }


    }
}
