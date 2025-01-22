using Chat.Api.Context;
using Chat.Api.Entities;
using Chat.Api.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Chat.Api.Repositories
{
    public class MessageRepository(ChatDbContext context) : IMessageRepository
    {
        private readonly ChatDbContext _context = context;

        public async Task<List<Message>> GetAllMessages()
        {
            var messages = await _context.Messages.Include(m=>m.Content).AsNoTracking().ToListAsync();

            return messages;
        }

        public async Task<List<Message>> GetChatMessages(Guid chatId)
        {
            var messages = await _context.Messages.Where(m=>m.ChatId==chatId).Include(m=>m.Content).ToListAsync();

            return messages;
        }

        public async Task<Message> GetMessageById(int messageId)
        {
            var message = await _context.Messages.Include(m=>m.Content).SingleOrDefaultAsync(m=>m.Id==messageId);

            if (message == null)
            {
                throw new MessageNotFoundException();
            }

            return message;
        }

        public async Task<Message> GetChatMessageById(Guid chatId, int messageId)
        {
            var message = await _context.Messages.Include(m=>m.Content).SingleOrDefaultAsync(m => m.Id == messageId && m.ChatId==chatId);

            if (message == null)
            {
                throw new MessageNotFoundException();
            }

            return message;
        }

        public async Task AddMessage(Message message)
        {
            _context.Messages.Add(message);

            await _context.SaveChangesAsync();
        }
    }
}
