using Chat.Api.Constants;
using Chat.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chat.Api.Context
{
    public class ChatDbContext:DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Entities.Chat> Chats { get; set; }

        public DbSet<UserChat> UsersChats { get; set; }

        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var password = "adminPassword";
            

            var user=new User()
            {
                Id = Guid.NewGuid(),
                Firstname = "Admin",
                Lastname = "Adminov",
                Username = "admin007",
                Gender = UserConstants.Male,
                Role = UserConstants.AdminRole,
            };

            var passwordHash = new PasswordHasher<User>().HashPassword(user,password);

            user.PasswordHash=passwordHash;


            //seed data

            modelBuilder.Entity<User>()
                .HasData(new List<User>()
                {
                    user
                });


        }
    }
}
