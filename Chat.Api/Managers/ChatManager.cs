using Chat.Api.DTOs;
using Chat.Api.Entities;
using Chat.Api.Extensions;
using Chat.Api.Helpers;
using Chat.Api.Repositories;

namespace Chat.Api.Managers
{
    public class ChatManager(IUnitOfWork unitOfWork, MemoryCacheManager memoryCacheManager)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly MemoryCacheManager _memoryCacheManager = memoryCacheManager;

        //For Admin
        public async Task<List<ChatDto>> GetAllChats()
        {
            var chats = await _unitOfWork.ChatRepository.GetAllChats();

            return chats.ParseToDtos();
        }


        public async Task<List<ChatDto>> GetAllUserChats(Guid userId)
        {
            var chats = await _unitOfWork.ChatRepository.GetAllUserChats(userId);

            return chats.ParseToDtos();
        }

        public async Task<ChatDto> GetUserChatById(Guid userId, Guid chatId)
        {
            var chat = await _unitOfWork.ChatRepository.GetUserChatById(userId, chatId);

            return chat.ParseToDto();
        }

        public async Task<ChatDto> AddOrEnterChat(Guid fromUserId, Guid toUserId)
        {
            var (check, chat) = await _unitOfWork.ChatRepository.CheckChatExist(fromUserId, toUserId);

            if (check)
            {
                return chat?.ParseToDto()!;
            }

            var fromUser = await _unitOfWork.UserRepository.GetUserByIdAsync(fromUserId);

            var toUser = await _unitOfWork.UserRepository.GetUserByIdAsync(toUserId);

            List<string> names = new List<string>()
            {
                StaticHelper.GetFullName(fromUser.Firstname,fromUser.Lastname),
                StaticHelper.GetFullName(toUser.Firstname,toUser.Lastname)
            };

            chat = new Entities.Chat()
            {
                ChatNames = names
            };

            await _unitOfWork.ChatRepository.AddChat(chat);


            var fromUserChat = new UserChat()
            {
                UserId = fromUserId,
                ChatId = chat.Id,
                ToUserId = toUserId
            };

            await _unitOfWork.UserChatRepository.AddUserChat(fromUserChat);

            var toUserChat = new UserChat()
            {
                UserId = toUserId,
                ChatId = chat.Id,
                ToUserId = fromUserId
            };

            await _unitOfWork.UserChatRepository.AddUserChat(toUserChat);


            return chat.ParseToDto();
        }


        public async Task<string> DeleteChat(Guid userId, Guid chatId)
        {

            var chat = await _unitOfWork.ChatRepository.GetUserChatById(userId, chatId);


            await _unitOfWork.ChatRepository.DeleteChat(chat);

            return "Deleted successfully";
        }


    }
}
