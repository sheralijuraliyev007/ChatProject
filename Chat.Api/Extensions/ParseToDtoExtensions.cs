using Chat.Api.DTOs;
using Chat.Api.Entities;
using Mapster;

namespace Chat.Api.Extensions
{
    public static class ParseToDtoExtensions
    {
        public static UserDto ParseToDto(this User user)
        {
            UserDto dto = user.Adapt<UserDto>();

            return dto;
        }


        public static List<UserDto> ParseToDtos(this List<User>? users)
        {
            //List<UserDto> dtos = users.Adapt<List<UserDto>>();

            //return dtos;

            List<UserDto> dtos = new();

            if (users is null || users.Count==0)
            {
                return dtos;
            }

            dtos.AddRange(users.Select(u=>u.ParseToDto()));

            return dtos;
        }


        public static ChatDto ParseToDto(this Entities.Chat chat)
        {
            var dto = chat.Adapt<ChatDto>();

            return dto;
        }


        public static List<ChatDto> ParseToDtos(this List<Entities.Chat> chats)
        {
            List<ChatDto> dtos = new();

            if (chats is null || chats.Count==0)
            {
                return dtos;
            }

            dtos.AddRange(chats.Select(ch=>ch.ParseToDto()));
            return dtos;
        }

        public static MessageDto ParseToDto(this Message message)
        {
            MessageDto dto = message.Adapt<MessageDto>();

            return dto;
        }


        public static List<MessageDto> ParseToDtos(this List<Entities.Message> messages)
        {
            var dtos = new List<MessageDto>();

            if (messages is null || messages.Count==0)
            {
                return dtos;
            }

            dtos.AddRange(messages.Select(m=>m.ParseToDto()));

            return dtos;
        }
    }
}
