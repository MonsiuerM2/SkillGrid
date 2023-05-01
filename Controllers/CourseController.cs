using DMed_Razor.Data;
using DMed_Razor.DTOs.CMEs;
using DMed_Razor.Entities;
using DMed_Razor.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DMed_Razor.Controllers
{
    public class CourseController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly CourseModuleHelper _cmHelper;
        private readonly AccountHelper _accHelper;

        public CourseController(DataContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _cmHelper = new CourseModuleHelper(_context);
            _accHelper = new AccountHelper(_userManager);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCourse(CourseCreateDto courseCreateDto)
        {
            if (await _cmHelper.CourseExists(courseCreateDto.Name))
            {
                return BadRequest("There is already a course with that name.");
            }
            if (courseCreateDto.ModulesList?.Count < 2)
            {
                return BadRequest("A course should have a minimum of 2 modules.");
            }

            Course newCourse = new Course()
            {
                Name = courseCreateDto.Name,
                ModulesList = new List<CourseModules>()
            };

            if (courseCreateDto.ModulesList?.Distinct().Count() != courseCreateDto.ModulesList?.Count())
            {
                return BadRequest("There cannot be duplicate modules in the modules list.");
            }

            List<Module> modulesList = await _context.Modules
                .Where(m => courseCreateDto.ModulesList != null && courseCreateDto.ModulesList.Contains(m.ModuleId))
                .ToListAsync();

            if (courseCreateDto.ModulesList != null && modulesList.Count() != courseCreateDto.ModulesList.Count())
            {
                return BadRequest("A course's modules should be modules that exist.");
            }

            foreach (var module in modulesList)
            {
                CourseModules courseModule = new CourseModules { CourseId = newCourse.CourseId, ModuleId = module.ModuleId };
                newCourse.ModulesList.Add(courseModule);
            }

            var result = await _context.Courses.AddAsync(newCourse);
            await _context.SaveChangesAsync();

            return Ok("Course successfully added");
        }

        [HttpGet("{CourseId}")]
        public async Task<ActionResult<Course>> GetCourse(int CourseId)
        {
            var course = await _context.Courses
               .Where(mp => mp.CourseId == CourseId)
               .Select(mp => new Course
               {
                   CourseId = mp.CourseId,
                   Name = mp.Name,
                   ModulesList = new List<CourseModules>()
               })
               .FirstOrDefaultAsync();

            if (course == null)
            {
                return BadRequest("No course with that ID exists.");
            }

            course.ModulesList = await
                _context.CourseModules
                        .Where(cm => cm.CourseId == CourseId)
                        .Select(cm => new CourseModules
                        {
                            CourseModulesId = cm.CourseModulesId,
                            CourseId = cm.CourseId,
                            ModuleId = cm.ModuleId,
                            Module = cm.Module
                        })
                        .ToListAsync();

            return course;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses(CoursesGetDto coursesGetDto)
        {
            var courses = await _context.Courses
               .Select(mp => new Course
               {
                   CourseId = mp.CourseId,
                   Name = mp.Name
               })
               .ToListAsync();

            var courseModules = await _context
                .CourseModules
                .Include(mp => mp.Module)
                .ToListAsync();

            foreach (var course in courses)
            {
                course.ModulesList = courseModules
                    .Where(cm => cm.CourseId == course.CourseId)
                    .Select(cm => new CourseModules
                    {
                        CourseModulesId = cm.CourseModulesId,
                        CourseId = cm.CourseId,
                        ModuleId = cm.ModuleId,
                        Module = coursesGetDto.IncludeModule ? cm.Module : null
                    })
                    .ToList();
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = JsonSerializer.Serialize(courses, options);

            //return Content(json, "application/json");

            return courses;
        }

        [HttpGet("mod-prereqs/{CourseId}")]
        public async Task<ActionResult<List<Module>>> GetCourseModulePreReqs(int CourseId)
        {
            if (!await _cmHelper.CourseExists(CourseId))
            {
                return BadRequest("No course with that ID exists");
            }
            List<int> ModulesIdsList = await
                _context.CourseModules
                        .Where(cm => cm.CourseId == CourseId)
                        .Select(cm => cm.ModuleId)
                        .ToListAsync();

            List<Module> courseModulePreReqsIds = await _context.ModulePreReqs
                    .Where(m => ModulesIdsList.Contains(m.ModuleId))
                    .Select(cm => cm.PreReq)
                    .Distinct()
                    .ToListAsync();

            /*if (course == null)
            {
                return BadRequest("No course with that ID exists.");
            }*/

            return courseModulePreReqsIds;
        }

        [HttpGet("mod-assign/{CourseId}")]
        public async Task<ActionResult<List<Module_ModuleAssignmentDto>>> GetModulesAssignments(int CourseId)
        {
            if (!await _cmHelper.CourseExists(CourseId))
            {
                return BadRequest("No course with that ID exists");
            }
            //
            List<int> moduleIdList = await _context.CourseModules
                        .Where(cm => cm.CourseId == CourseId)
                        .Select(cm => cm.ModuleId)
                        .ToListAsync();

            List<Module> modulesList = await _context.Modules
                .Where(m => moduleIdList.Contains(m.ModuleId))
                .Select(mp => new Module
                {
                    ModuleId = mp.ModuleId,
                    maxDurationHours = mp.maxDurationHours,
                    Name = mp.Name
                })
                .ToListAsync();

            List<Module_ModuleAssignmentDto> module_ModuleAssignmentDtoList = new();

            foreach (Module currModule in modulesList)
            {
                List<ModuleAssignment> moduleAssignments = await _context.ModuleAssignments
                        //.Include(m => m.Lecturer)
                        .Where(ma => ma.ModuleId == currModule.ModuleId).ToListAsync();

                Module_ModuleAssignmentDto module_ModuleAssignmentDto = new()
                {
                    Module = currModule,
                    ModuleAssignments = moduleAssignments
                };

                module_ModuleAssignmentDtoList.Add(module_ModuleAssignmentDto);
            }

            /*if (course == null)
            {
                return BadRequest("No course with that ID exists.");
            }*/

            return module_ModuleAssignmentDtoList;
        }

        [HttpGet("get-registrations")]
        public async Task<ActionResult<IEnumerable<CourseRegistration>>> GetRegistrations(RegistrationGetDto registrationGetDto)
        {
            var registrationsQuery = _context.CourseRegistrations.AsQueryable();

            if (registrationGetDto.IncludeStudent)
                registrationsQuery = registrationsQuery.Include(m => m.Student);

            var registrations = await registrationsQuery.ToListAsync();
            return registrations;
        }

        [HttpGet("get-all-regs/{studentId}")]
        public async Task<ActionResult> GetAllRegistrations(int studentId)
        {
            if (!await _accHelper.UserExists(studentId, "Student"))
            {
                return BadRequest("No student with that ID exists.");
            }

            var courseRegistrations = await _context.CourseRegistrations
                .Where(cr => cr.StudentId == studentId)
                .Select(cr => new CourseRegistration
                {
                    CourseRegId = cr.CourseRegId,
                    CourseId = cr.CourseId,
                    StudentId = cr.StudentId,
                    Completed = cr.Completed,
                    CourseEnrolledtDate = cr.CourseEnrolledtDate
                })
                .ToListAsync();

            var moduleRegistrations = await _context.ModuleRegistrations
                .Where(cr => cr.StudentId == studentId)
                .Select(cr => new ModuleRegistration
                {
                    ModuleRegId = cr.ModuleRegId,
                    LectureId = cr.LectureId,
                    StudentId = cr.StudentId,
                    StartDate = cr.StartDate,
                    EndDate = cr.EndDate
                })
                .ToListAsync();


            var result = new
            {
                Message = "These are the lists of Modules and Course you have registered for.",
                ModuleRegistrations = moduleRegistrations,
                CourseRegistrations = courseRegistrations
            };

            return Ok(result);

        }



        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollCourse(CourseEnrollDto courseEnrollDto)
        {
            if (!await _cmHelper.CourseExists(courseEnrollDto.CourseId))
            {
                return BadRequest("No course with that ID exists.");
            }
            if (!await _accHelper.UserExists(courseEnrollDto.StudentId, "Student"))
            {
                return BadRequest("No student with that ID exists.");
            }
            if (await _cmHelper.CourseAlreadyRegistered(courseEnrollDto.CourseId, courseEnrollDto.StudentId))
            {
                return BadRequest("You have already registered for that course.");
            }

            Course? course = (await GetCourse(courseEnrollDto.CourseId)).Value;

            List<int>? modulesIdList = courseEnrollDto.ModuleMAs?.Select(pair => pair.Key).ToList();

            var modules = (course.ModulesList.Where(c => !modulesIdList.Contains(c.ModuleId)).ToList());

            if (modules.Any())
            {
                return BadRequest(new
                {
                    message = "You have not listed the module assignment of these modules.",
                    modulesList = modules
                });
            }

            List<Module> moduleList = new();

            foreach (KeyValuePair<int, int> pair in courseEnrollDto.ModuleMAs)
            {
                if (!await _context.ModuleAssignments
                    .Where(ma => ma.ModuleId == pair.Key && ma.AssignmentId == pair.Value)
                    .AnyAsync())
                {
                    moduleList.Add(await _context.Modules.FirstOrDefaultAsync(m => m.ModuleId == pair.Key));
                }
            }

            if (moduleList.Any())
            {
                return BadRequest(new
                {
                    message = "You have selected module assignment(s) for module(s) that do not exist",
                    module = moduleList
                });
            }

            List<Module> modulePreReqList = (await GetCourseModulePreReqs(courseEnrollDto.CourseId)).Value;

            var completedLectureIds = await _context.ModuleRegistrations
                .Where(reg => reg.Student.Id == courseEnrollDto.StudentId && reg.Completed)
                .Select(reg => reg.LectureId)
                .ToListAsync();

            var completedModuleIds = await _context.ModuleAssignments
                .Where(ma => completedLectureIds.Contains(ma.AssignmentId))
                .Select(ma => ma.ModuleId)
                .ToListAsync();

            List<int> notCompletedPrerequisiteIds = modulePreReqList
                .Where(p => !completedModuleIds.Contains(p.ModuleId)).Select(p => p.ModuleId).ToList();

            if (notCompletedPrerequisiteIds.Any())
            {
                var notCompletedPrerequisites = await _context.Modules
                    .Where(m => notCompletedPrerequisiteIds.Contains(m.ModuleId))
                    .ToListAsync();

                return BadRequest(new
                {
                    message = "You have not completed the courses's prerequisites. You have not completed these modules.",
                    modulesList = notCompletedPrerequisites
                });
            }

            AppUser? student = await _userManager.FindByIdAsync(courseEnrollDto.StudentId.ToString());
            moduleList = new();

            foreach (KeyValuePair<int, int> pair in courseEnrollDto.ModuleMAs)
            {

                var isEnrolled = await _context.ModuleRegistrations
                .AnyAsync(r => r.Student.Id == courseEnrollDto.StudentId && r.LectureId == pair.Value);

                if (isEnrolled) moduleList.Add(await _context.Modules.FirstOrDefaultAsync(m => m.ModuleId == pair.Key));

                var moduleRegistration = new ModuleRegistration
                {
                    Student = student,
                    LectureId = pair.Value,
                    StudentId = student.Id,
                    Completed = false
                };
                await _context.ModuleRegistrations.AddAsync(moduleRegistration);

            }

            if (moduleList.Any())
            {
                return BadRequest(new
                {
                    message = "You have selected to enroll in module(s) which you have already registered in with the same teacher." +
                    "You can select to enroll in the same module with another teacher or leave it empty because you have already registered it",
                    module = moduleList
                });
            }

            var courseRegistration = new CourseRegistration
            {
                Student = student,
                StudentId = student.Id,
                Completed = false,
                CourseId = course.CourseId
            };
            await _context.CourseRegistrations.AddAsync(courseRegistration);
            await _context.SaveChangesAsync();
            return Ok("You have successfully enrolled in the course.");
        }


    }

}
