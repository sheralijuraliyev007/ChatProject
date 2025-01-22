using Chat.Api.Context;
using Chat.Api.Entities;
using Chat.Api.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Chat.Api.Repositories
{
    public class UserChatRepository:IUserChatRepository
    {
        private readonly ChatDbContext _context;

        public UserChatRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task AddUserChat(UserChat userChat)
        {
            _context.UsersChats.Add(userChat);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserChat(UserChat userChat)
        {
            _context.UsersChats.Remove(userChat);

            await _context.SaveChangesAsync();
        }

        public async Task InUserChat(Guid chatId, Guid userId)
        {
            //var userChat =
            //    await _context.UsersChats.SingleOrDefaultAsync(userChat =>
            //        userChat.ChatId == chatId);

            //if (userChat is null)
            //{
            //    throw new ChatNotFoundException();
            //}

            //userChat =
            //    await _context.UsersChats.SingleOrDefaultAsync(userChat =>
            //        userChat.ChatId == chatId && userChat.UserId==userId);


            //if (userChat is null)
            //{
            //    throw new ChatPrivacyException();
            //}


            var userChat = await _context.UsersChats
                .SingleOrDefaultAsync(uc => uc.UserId == userId
                                            && uc.ChatId == chatId);

            if (userChat is null)
                throw new Exception("Not found chat");
        }

    }
}
