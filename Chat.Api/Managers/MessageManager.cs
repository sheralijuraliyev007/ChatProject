using Chat.Api.DTOs;
using Chat.Api.Entities;
using Chat.Api.Exceptions;
using Chat.Api.Extensions;
using Chat.Api.Models;
using Chat.Api.Repositories;
 

namespace Chat.Api.Managers
{
    public class MessageManager(IUnitOfWork unitOfWork, IHostEnvironment hostingEnvironment)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        
        private readonly IHostEnvironment _hostingEnvironment = hostingEnvironment;


        //Admin Action
        public async Task<List<MessageDto>> GetAllMessages()
        {

            var messages= await _unitOfWork.MessageRepository.GetAllMessages();

            return messages.ParseToDtos();
        }

        public async Task<List<MessageDto>> GetChatMessages(Guid chatId)
        {
            var messages = await _unitOfWork.MessageRepository.GetChatMessages(chatId);

            return messages.ParseToDtos();
        }


        //Admin Action
        public async Task<MessageDto> GetMessageById(int messageId)
        {
            var message = await _unitOfWork.MessageRepository.GetMessageById(messageId);


            return message.ParseToDto();
        } 



        public async Task<MessageDto> GetChatMessageById(Guid chatId, int messageId)
        {

            var message = await _unitOfWork.MessageRepository.GetChatMessageById(chatId, messageId);

            return message.ParseToDto();
        }

        public async Task<MessageDto> SendTextMessage(Guid chatId,Guid userId, TextModel model)
        {

            await _unitOfWork.UserChatRepository.InUserChat(chatId, userId);


            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);

            


            var message = new Message()
            {
                FromUserId = userId,
                FromUserName = user.Username,
                ChatId = chatId,
                Text = model.Text,
            };

            await _unitOfWork.MessageRepository.AddMessage(message);

            return message.ParseToDto();
        }

        public async Task<MessageDto> SendFileMessage(Guid userId, Guid chatId, FileModel model)
        {
            
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            
            await _unitOfWork.UserChatRepository.InUserChat(chatId, userId);
        
            var ms = new MemoryStream();
            
            await model.File.CopyToAsync(ms);
            
            var data= ms.ToArray();


            var fileUrl = GetFilePath();
            
            
            await File.WriteAllBytesAsync(fileUrl, data);
            
            
            var content = new Content()
            {
                FileUrl = fileUrl,
                Type = model.File.ContentType,
                
            };


            var message = new Message()
            {
                FromUserId = userId,
                ChatId = chatId,
                FromUserName = user.Username,
                Content = content,
                ContentId = content.Id

            }; 
            
            await _unitOfWork.MessageRepository.AddMessage(message);
            
            return message.ParseToDto();
        }

        public string GetFilePath()
        {
            var generalPath=_hostingEnvironment.ContentRootPath;

            var fileName = generalPath + "\\wwwroot\\MessageFiles\\" + Guid.NewGuid() ;
            return fileName;
        }

    }
}
