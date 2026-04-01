using ChatAppServer.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppServer.WebAPI.Context
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}
