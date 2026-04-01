using ChatAppServer.WebAPI.Context;
using ChatAppServer.WebAPI.Dtos;
using ChatAppServer.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class ChatsController
        (AppDbContext context): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetChats
            (Guid userId, Guid toUserId, CancellationToken cancellationToken)
        {
            List<Chat> chats = await context
                .Chats
                .Where(p=>
                p.UserId == userId && p.ToUserId == toUserId ||
                p.ToUserId == userId && p.UserId == toUserId)
                .OrderBy(p=>p.Date)
                .ToListAsync(cancellationToken);

            return Ok(chats);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage
            (SendMessageDto request, CancellationToken cancellationToken)
        {
            Chat chat = new()
            {
                UserId = request.UserId,
                ToUserId = request.ToUserId,
                Message = request.Message,
                Date = DateTime.Now
            };

            await context.AddAsync(chat);
            await context.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
