using Chat.Api.DTOs;
using Chat.Api.Entities;
using Chat.Api.Exceptions;
using Chat.Api.Extensions;
using Chat.Api.Hubs;
using Chat.Api.Models;
using Chat.Api.Repositories;
using Microsoft.AspNetCore.SignalR;


namespace Chat.Api.Managers
{
    public class MessageManager(IUnitOfWork unitOfWork, IHostEnvironment hostingEnvironment, IHubContext<ChatHub> context)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        
        private readonly IHostEnvironment _hostingEnvironment = hostingEnvironment;

        private readonly IHubContext<ChatHub> _hubContext = context;


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


            //await _hubContext.Clients.All.SendAsync("NewMessage", message.ParseToDto());


            var connection1 = ConnectionIdService.ConnectionIds.FirstOrDefault(c => c.Item1 == userId);

            var userChat = await _unitOfWork.UserChatRepository.GetUserChat(chatId:chatId,userId:userId);


            var connection2 = ConnectionIdService.ConnectionIds.FirstOrDefault(c => c.Item1 == userChat.ToUserId);



            if (!string.IsNullOrEmpty(connection1?.Item2))
            {
                await _hubContext.Clients.Client(connection1.Item2).SendAsync("NewMessage", message.ParseToDto());

            }


            if (!string.IsNullOrEmpty(connection2?.Item2))
            {
                await _hubContext.Clients.Client(connection2.Item2).SendAsync("NewMessage", message.ParseToDto());

            }

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
