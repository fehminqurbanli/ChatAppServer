using ChatAppServer.WebAPI.Context;
using ChatAppServer.WebAPI.Dtos;
using ChatAppServer.WebAPI.Models;
using GenericFileService.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController
        (AppDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register
            (RegisterDto request,
            CancellationToken cancellationToken)
        {
            bool isNameExist = await context.Users
                .AnyAsync(u => u.Name == request.Name, cancellationToken);

            if (isNameExist)
                return BadRequest(new { Message = "This user has been used" });

            string avatar = FileService.FileSaveToServer(request.File, "wwwroot/avatar/");
            User user = new()
            {
                Name = request.Name,
                Avatar = avatar,
            };

            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Login(
            string name,
            CancellationToken cancellationToken)
        {
            User? user = await context.Users
                .FirstOrDefaultAsync(u => u.Name == name, cancellationToken);

            if (user is null)
                return BadRequest(new { Message = "User not found" });

            user.Status = "Online";
            await context.SaveChangesAsync(cancellationToken);

            return Ok(user);
        }
    }
}
