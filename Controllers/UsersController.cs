using DMed_Razor.Data;
using DMed_Razor.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMed_Razor.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        //private readonly ILogger<UsersController> _logger;
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UsersController(DataContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            //_logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            return users;
        }

        [HttpGet("{UserId}")]
        public async Task<ActionResult<AppUser>> GetUser(int UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());

            return user;
        }
    }
}