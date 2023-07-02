using DMed_Razor.Data;
using DMed_Razor.Entities;
using Microsoft.EntityFrameworkCore;

namespace DMed_Razor.Helpers
{
    public class CourseModuleHelper
    {
        private readonly DataContext _context;

        public CourseModuleHelper(DataContext context)
        {
            _context = context;
        }

        public async Task<List<string>?> GetModulesNamesAsync(int courseId)
        {
            var course = await _context.Courses
               .Where(mp => mp.CourseId == courseId)
               .Select(mp => new Course
               {
                   CourseId = mp.CourseId,
                   Name = mp.Name,
                   ModulesList = new List<CourseModules>(),
               })
               .FirstOrDefaultAsync();

            if (course == null)
            {
                return null;
            }

            course.ModulesList = await
            _context.CourseModules
                        .Where(cm => cm.CourseId == courseId)
                        .Select(cm => new CourseModules
                        {
                            CourseModulesId = cm.CourseModulesId,
                            CourseId = cm.CourseId,
                            ModuleId = cm.ModuleId,
                            Module = cm.Module
                        })
                        .ToListAsync();

            var modulesNames = new List<string>();

            foreach (var cm in course.ModulesList)
            {
                modulesNames.Add(cm.Module.Name);
            }

            return modulesNames;
        }

        public async Task<bool> ModuleExists(string name)
        {
            return await _context.Modules.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }
        public async Task<bool> ModuleExists(int moduleId)
        {
            return await _context.Modules.AnyAsync(x => x.ModuleId == moduleId);
        }

        public async Task<bool> CourseExists(string name)
        {
            return await _context.Courses.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CourseExists(int courseId)
        {
            return await _context.Courses.AnyAsync(cm => cm.CourseId == courseId);
        }

        public async Task<bool> CourseAlreadyRegistered(int courseId, int studentId)
        {
            return await _context.CourseRegistrations.Where(cm => cm.CourseId == courseId && cm.StudentId == studentId).AnyAsync();
        }
    }


}
