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
    public class ModuleController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly CourseModuleHelper _cmHelper;
        private readonly UserManager<AppUser> _userManager;

        public ModuleController(DataContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _cmHelper = new CourseModuleHelper(_context);
        }


        [HttpGet("{ModuleId}")]
        public async Task<ActionResult<ModuleViewDto>> GetModule(int ModuleId)
        {
            var module = await _context.Modules
               .Select(mp => new ModuleViewDto
               {
                   ModuleId = mp.ModuleId,
                   MaxDurationHours = mp.maxDurationHours,
                   Name = mp.Name
               })
               .SingleOrDefaultAsync(mp => mp.ModuleId == ModuleId);

            if (module == null)
            {
                return BadRequest("No module with that ID exists.");
            }

            module.ModulePreReqs = await _context
                    .ModulePreReqs
                    .Include(mp => mp.PreReq)
                    .Where(mp => mp.ModuleId == module.ModuleId)
                    .Select(mp => new ModulePreReqsDto
                    {
                        ModulePreReqsId = mp.ModulePreReqsId,
                        ModuleId = mp.ModuleId,
                        PreReqId = mp.PreReqId/*,
                        PreReq = new Module
                        {
                            ModuleId = mp.PreReq.ModuleId,
                            Name = mp.PreReq.Name
                        }*/
                    })
                    .ToListAsync();

            return module;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleViewDto>>> GetModules()
        {
            var modules = await _context.Modules
                .Select(mp => new ModuleViewDto
                {
                    ModuleId = mp.ModuleId,
                    MaxDurationHours = mp.maxDurationHours,
                    Name = mp.Name
                })
                .ToListAsync();
            var modulePreReqs = await _context
                .ModulePreReqs
                .Include(mp => mp.PreReq)
                .ToListAsync();

            foreach (var module in modules)
            {
                module.ModulePreReqs = modulePreReqs
                    .Where(mp => mp.ModuleId == module.ModuleId)
                    .Select(mp => new ModulePreReqsDto
                    {
                        ModulePreReqsId = mp.ModulePreReqsId,
                        ModuleId = mp.ModuleId,
                        PreReqId = mp.PreReqId/*,
                        PreReq = new Module
                        {
                            ModuleId = mp.PreReq.ModuleId,
                            Name = mp.PreReq.Name
                        }*/
                    })
                    .ToList();
            }
            //modules = await modules.
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = JsonSerializer.Serialize(modules, options);

            //return Content(json, "application/json");

            return modules;

        }

        [HttpPost("add")]
        public async Task<IActionResult> AddModule(ModuleDto moduleDto)
        {
            if (await _cmHelper.ModuleExists(moduleDto.Name))
            {
                return BadRequest("There is already a course with that name.");
            }

            Entities.Module newModule = new Entities.Module()
            {
                Name = moduleDto.Name,
                maxDurationHours = moduleDto.MaxDurationHours,
                ModulePreReqs = new List<ModulePreReqs>()
            };

            if (moduleDto.ModulesPreReq?.Distinct().Count() != moduleDto.ModulesPreReq?.Count())
            {
                return BadRequest("There cannot be duplicate modules in the prerequisites.");
            }

            var preReqs = await _context.Modules
                .Where(m => moduleDto.ModulesPreReq != null && moduleDto.ModulesPreReq.Contains(m.ModuleId))
                .ToListAsync();

            if (moduleDto.ModulesPreReq != null && preReqs.Count() != moduleDto.ModulesPreReq.Count())
            {
                return BadRequest("A module's prerequisites should be modules that exist.");
            }

            foreach (var preReq in preReqs)
            {
                var modulePreReq = new ModulePreReqs { ModuleId = newModule.ModuleId, PreReqId = preReq.ModuleId };
                newModule.ModulePreReqs.Add(modulePreReq);
            }

            var result = await _context.Modules.AddAsync(newModule);
            await _context.SaveChangesAsync();

            //if (result.) return BadRequest(result.Errors);

            return Ok("Module successfully added");
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollModule(EnrollDto enrollDto)
        {
            ModuleAssignment moduleAssignment = await _context.ModuleAssignments
                .FirstOrDefaultAsync(ma => ma.AssignmentId == enrollDto.AssignmentId);

            if (moduleAssignment == null)
            {
                return NotFound("There is no module assignment with that ID");
            }

            if (await _context.Registration
                .AnyAsync(r => r.Student.Id == enrollDto.StudentId && r.LectureId == enrollDto.AssignmentId && r.isModule))
            {
                return BadRequest("You are already enrolled in that course/module.");
            }

            ModuleViewDto module = (await GetModule(moduleAssignment.ModuleId)).Value;

            if (module == null)
            {
                return NotFound("There is no module with that ID");
            }

            AppUser student = await _userManager.FindByIdAsync(enrollDto.StudentId.ToString());

            if (student == null || !(await _userManager.IsInRoleAsync(student, "Student")))
            {
                return NotFound("There is no student with that ID");
            }

            List<int> studentCompletedModules = await _context.Registration
                .Where(reg => reg.isModule && reg.Student.Id == enrollDto.StudentId && reg.Completed)
                .Select(reg => reg.LectureId)
                .ToListAsync();

            var studentCompletedModulesIds = (await _context.ModuleAssignments
                .Where(m => studentCompletedModules.Contains(m.AssignmentId))
                .Select(m => m.ModuleId)
                .ToListAsync());

            List<int> notCompletedPrerequisiteIds = module.ModulePreReqs
                .Where(p => !studentCompletedModulesIds.Contains(p.PreReqId))
                .Select(p => p.PreReqId)
                .ToList();

            if (notCompletedPrerequisiteIds.Any())
            {
                List<Module> notCompletedPrerequisites = await _context.Modules
                    .Where(m => notCompletedPrerequisiteIds.Contains(m.ModuleId))
                    .ToListAsync();

                return BadRequest(new
                {
                    message = "You have not completed the module's prerequisites. You have not completed these modules.",
                    modulesList = notCompletedPrerequisites
                });
            }

            Registration registration = new Registration
            {
                Student = await _userManager.FindByIdAsync(enrollDto.StudentId.ToString()),
                LectureId = enrollDto.AssignmentId,
                isModule = true,
                StudentId = enrollDto.StudentId,
                Completed = false
            };

            await _context.Registration.AddAsync(registration);
            await _context.SaveChangesAsync();

            return Ok("You have successfully enrolled in the module.");

        }

        [HttpGet("get-registrations")]
        public async Task<ActionResult<IEnumerable<Registration>>> GetRegistrations(RegistrationGetDto registrationGetDto)
        {
            var registrationsQuery = _context.Registration.AsQueryable();

            if (registrationGetDto.IncludeStudent)
                registrationsQuery = registrationsQuery.Include(m => m.Student);

            var registrations = await registrationsQuery.ToListAsync();
            return registrations;
        }

        [HttpPost("complete-module")]
        public async Task<IActionResult> CompleteModule(CompleteModuleDto completeModuleDto)
        {
            Registration? studentModule = await _context.Registration
                .Include(reg => reg.Student)
                .Where(reg => reg.isModule)
                .SingleOrDefaultAsync(r => r.Student.Id == completeModuleDto.StudentId
                    && r.LectureId == completeModuleDto.LectureId);

            if (studentModule == null)
            {
                return NotFound("You have not enrolled in this course/module.");
            }
            if (studentModule.Completed)
            {
                return BadRequest("You have already completed this course/module.");
            }
            studentModule.Completed = true;
            await _context.SaveChangesAsync();

            return Ok("You have successfully complete this module.");
        }

        [HttpDelete("deregister")]
        public async Task<IActionResult> Deregister(DeregisterDto deregisterDto)
        {
            Registration? studentModule = await _context.Registration
                .Include(reg => reg.Student)
                .Where(reg => reg.isModule)
                .SingleOrDefaultAsync(r => r.Student.Id == deregisterDto.StudentId
                    && r.LectureId == deregisterDto.LectureId);

            if (studentModule == null)
            {
                return NotFound("You have not enrolled in that course.");
            }

            _context.Registration.Remove(studentModule);
            await _context.SaveChangesAsync();

            return Ok("You have successfully deregistered yourself from this module.");
        }
    }
}
