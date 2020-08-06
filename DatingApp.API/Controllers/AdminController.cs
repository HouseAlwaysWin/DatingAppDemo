using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers {

    [ApiController]
    [Route ("api/[controller]")]
    public class AdminController : ControllerBase {
        private readonly DataContext _context;

        public AdminController (DataContext context) {
            _context = context;

        }

        [Authorize (Policy = "RequireAdminRole")]
        [HttpGet ("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles () {

            var userList = await _context.Users
                .OrderBy (x => x.UserName)
                .Select (user => new {
                    Id = user.Id,
                        UserName = user.UserName,
                        Roles = user.UserRoles.Join (_context.Roles,
                            ur => ur.RoleId, r => r.Id,
                            (ur, r) => r.Name).ToList ()
                }).ToListAsync ();
            return Ok (userList);
        }

        [Authorize (Policy = "ModeratePhotoRole")]
        [HttpGet ("photosForModeration")]
        public IActionResult GetPhotosForModeration () {
            return Ok ("Admins or moderators can see this");
        }
    }
}