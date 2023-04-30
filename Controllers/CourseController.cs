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

        public CourseController(DataContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _cmHelper = new CourseModuleHelper(_context);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddModule(CourseCreateDto courseCreateDto)
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


    }

}
