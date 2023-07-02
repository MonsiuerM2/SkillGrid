using DMed_Razor.Data;
using DMed_Razor.DTOs.CMEs;
using DMed_Razor.Entities;
using DMed_Razor.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DMed_Razor.Controllers
{
    public class ModuleController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly CourseModuleHelper _cmHelper;
        private readonly AccountHelper _accHelper;
        private readonly UserManager<AppUser> _userManager;

        public ModuleController(DataContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _cmHelper = new CourseModuleHelper(_context);
            _accHelper = new AccountHelper(userManager, roleManager);
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
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> EnrollModule(ModuleEnrollDto moduleEnrollDto)
        {
            var moduleAssignment = await _context.ModuleAssignments
                .FirstOrDefaultAsync(ma => ma.AssignmentId == moduleEnrollDto.AssignmentId);

            if (moduleAssignment == null) return NotFound("There is no module assignment with that ID");

            var isEnrolled = await _context.ModuleRegistrations
                .AnyAsync(r => r.Student.Id == moduleEnrollDto.StudentId && r.LectureId == moduleEnrollDto.AssignmentId);
            if (isEnrolled) return BadRequest("You are already enrolled in that course/module with the same teacher. " +
                "You could enroll in the same module if another lecturer is teaching it.");

            var module = await GetModule(moduleAssignment.ModuleId);

            if (module == null) return NotFound("There is no module with that ID");

            if (!await _accHelper.UserExists(moduleEnrollDto.StudentId, "Student"))
            {
                return BadRequest("No student with that ID exists");
            }

            AppUser student = await _userManager.FindByIdAsync(moduleEnrollDto.StudentId.ToString());

            var completedLectureIds = await _context.ModuleRegistrations
                .Where(reg => reg.Student.Id == moduleEnrollDto.StudentId && reg.Completed)
                .Select(reg => reg.LectureId)
                .ToListAsync();

            var completedModuleIds = await _context.ModuleAssignments
                .Where(ma => completedLectureIds.Contains(ma.AssignmentId))
                .Select(ma => ma.ModuleId)
                .ToListAsync();

            List<int> notCompletedPrerequisiteIds = module.Value.ModulePreReqs
                .Where(p => !completedModuleIds.Contains(p.PreReqId)).Select(p => p.PreReqId).ToList();

            if (notCompletedPrerequisiteIds.Any())
            {
                var notCompletedPrerequisites = await _context.Modules
                    .Where(m => notCompletedPrerequisiteIds.Contains(m.ModuleId))
                    .ToListAsync();

                return BadRequest(new
                {
                    message = "You have not completed the module's prerequisites. You have not completed these modules.",
                    modulesList = notCompletedPrerequisites
                });
            }

            var registration = new ModuleRegistration
            {
                Student = student,
                LectureId = moduleEnrollDto.AssignmentId,
                StudentId = moduleEnrollDto.StudentId,
                Completed = false
            };
            await _context.ModuleRegistrations.AddAsync(registration);
            await _context.SaveChangesAsync();
            return Ok("You have successfully enrolled in the module.");
        }

        [HttpGet("get-registrations")]
        public async Task<ActionResult<IEnumerable<ModuleRegistration>>> GetRegistrations(RegistrationGetDto registrationGetDto)
        {
            var registrationsQuery = _context.ModuleRegistrations.AsQueryable();

            if (registrationGetDto.IncludeStudent)
                registrationsQuery = registrationsQuery.Include(m => m.Student);

            var registrations = await registrationsQuery.ToListAsync();
            return registrations;
        }

        [HttpPost("complete-module")]
        public async Task<IActionResult> CompleteModule(CompleteModuleDto completeModuleDto)
        {
            ModuleRegistration? studentModule = await _context.ModuleRegistrations
                .Include(reg => reg.Student)
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
            ModuleRegistration? studentModule = await _context.ModuleRegistrations
                .Include(reg => reg.Student)
                .SingleOrDefaultAsync(r => r.Student.Id == deregisterDto.StudentId
                    && r.LectureId == deregisterDto.LectureId);

            if (studentModule == null)
            {
                return NotFound("You have not enrolled in that course.");
            }

            _context.ModuleRegistrations.Remove(studentModule);
            await _context.SaveChangesAsync();

            return Ok("You have successfully deregistered yourself from this module.");
        }
    }
}
