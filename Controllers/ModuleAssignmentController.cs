using DMed_Razor.Data;
using DMed_Razor.DTOs.CMEs;
using DMed_Razor.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMed_Razor.Controllers
{

    public class ModuleAssignmentController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        //private readonly UsersController _usersController;
        public ModuleAssignmentController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleAssignment>>> GetModuleAssignments(ModAssignGetDto modAssignGetDto)
        {
            var moduleAssignmentsQuery = _context.ModuleAssignments.AsQueryable();

            if (modAssignGetDto.IncludeMod)
                moduleAssignmentsQuery = moduleAssignmentsQuery.Include(m => m.Module);

            if (modAssignGetDto.IncludeLecturer)
                moduleAssignmentsQuery = moduleAssignmentsQuery.Include(m => m.Lecturer);

            var moduleAssignments = await moduleAssignmentsQuery.ToListAsync();
            return moduleAssignments;
        }

        [HttpPost("add-mod-assign")]
        public async Task<IActionResult> AddModuleAssignment(ModAssignDto modAcssignDto)
        {
            var module = await _context.Modules
                .SingleOrDefaultAsync(m => m.ModuleId == modAcssignDto.ModuleId);
            var user = await _userManager.FindByIdAsync(modAcssignDto.LecturerId.ToString());
            var userRoles = await _userManager.GetRolesAsync(user);

            if (module == null)
            {
                return NotFound("There is no module with that ID");
            }
            else if (user == null || !userRoles.Contains("Lecturer"))
            {
                return NotFound("There is no lecturer with that ID");
            }

            List<ModuleAssignment> moduleAssignments = await _context.ModuleAssignments
                .Where(ma => ma.ModuleId == modAcssignDto.ModuleId
                    && ma.LecturerId == modAcssignDto.LecturerId).ToListAsync();

            if (moduleAssignments.Count() > 0)
            {
                return BadRequest("There is already an assignment with that module and lecturer");
            }

            ModuleAssignment moduleAssignment = new ModuleAssignment()
            {
                Module = module,
                Lecturer = user
            };

            await _context.ModuleAssignments.AddAsync(moduleAssignment);
            await _context.SaveChangesAsync();

            return Ok("Module Assignment succesfully added.");
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteModuleAssingment(DeleteMaDto deleteMaDto)
        {
            ModuleAssignment? moduleAssignment = await _context.ModuleAssignments
                .SingleOrDefaultAsync(r => r.AssignmentId == deleteMaDto.AssignmentId);

            if (moduleAssignment == null)
            {
                return NotFound("There is no module assignment with that ID.");
            }

            _context.ModuleAssignments.Remove(moduleAssignment);
            await _context.SaveChangesAsync();

            return Ok("You have successfully delete this module assigment.");
        }
    }
}
