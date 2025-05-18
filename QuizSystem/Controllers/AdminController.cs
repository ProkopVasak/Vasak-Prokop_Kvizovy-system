using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizSystem.Models;
using QuizSystem.Services;
using System.Security.Claims;

namespace QuizSystem.Controllers
{
    // Tento controller je přístupný pouze pro uživatele s 'AdminPolicy' autorizační politikou

    [Authorize(Policy = "AdminPolicy")]
    [Route("api/[controller]")]
    [ApiController]

    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // Konstruktor pro závislosti
        public AdminController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Získá všechny uživatele a jejich role

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            var userRoles = new List<object>();

            foreach (var user in users)
            {
                var roles = await _context.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToListAsync();

                userRoles.Add(new
                {
                    User = user,
                    Roles = roles
                });
            }

            return Ok(userRoles);
        }

        // Změní roli uživatele

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] ChangeRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { Message = "Uživatel nebyl nalezen." });
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeRolesResult.Succeeded)
            {
                return BadRequest(new { Message = "Nepodařilo se odebrat role." });
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, request.NewRole);
            if (!addRoleResult.Succeeded)
            {
                return BadRequest(new { Message = "Nepodařilo se přidat novou roli." });
            }

            if (request.NewRole == "Admin")
            {
                var adminClaim = new Claim("Admin", "True");
                var addClaimResult = await _userManager.AddClaimAsync(user, adminClaim);
                if (!addClaimResult.Succeeded)
                {
                    return BadRequest(new { Message = "Nepodařilo se přidat claim Admin." });
                }
            }
            else
            {
                var claims = await _userManager.GetClaimsAsync(user);
                var adminClaim = claims.FirstOrDefault(c => c.Type == "Admin" && c.Value == "True");
                if (adminClaim != null)
                {
                    var removeClaimResult = await _userManager.RemoveClaimAsync(user, adminClaim);
                    if (!removeClaimResult.Succeeded)
                    {
                        return BadRequest(new { Message = "Nepodařilo se odebrat claim Admin." });
                    }
                }
            }

            return Ok(new { Message = "Role byla úspěšně změněna." });
        }

        // Smaže uživatele podle ID

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            // Získáme uživatele podle Id
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Vrátíme 404 Not Found, pokud uživatel nebyl nalezen
                return NotFound(new { message = "Uživatel nebyl nalezen." });
            }

            // Pokusíme se uživatele smazat
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // Pokud operace selže, vrátíme 500 Internal Server Error s detaily
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Chyba při mazání uživatele.",
                    errors = result.Errors
                });
            }

            // Při úspěšném smazání vrátíme 204 No Content
            return NoContent();
        }
    }
}