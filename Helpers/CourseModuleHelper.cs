using DMed_Razor.Data;
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

        public async Task<bool> ModuleExists(string name)
        {
            return await _context.Modules.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CourseExists(string name)
        {
            return await _context.Courses.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CourseExists(int courseId)
        {
            return await _context.Courses.Where(cm => cm.CourseId == courseId).AnyAsync();
        }
    }


}
