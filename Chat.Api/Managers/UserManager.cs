using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Chat.Api.Constants;
using Chat.Api.DTOs;
using Chat.Api.Entities;
using Chat.Api.Exceptions;
using Chat.Api.Extensions;
using Chat.Api.Helpers;
using Chat.Api.Models.UserModels;
using Chat.Api.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Api.Managers
{
    public class UserManager(IUnitOfWork unitOfWork, JWTManager jwtManager, MemoryCacheManager memoryCache)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork; 
        private readonly JWTManager _jwtManager = jwtManager;
        private readonly MemoryCacheManager _memoryCacheManager = memoryCache;
        

        private const string Key = "users"; 

        public async Task<List<UserDto>> GetAllUsers()
        {

            var dtos = _memoryCacheManager.GetDtos(Key);

            if (dtos is not null)
            {
                return (List<UserDto>)dtos;
            }


            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();

            await Set();




            return users.ParseToDtos();
        }


        public async Task<UserDto> GetUserById(Guid id)
        {

            var dtos = _memoryCacheManager.GetDtos(Key);

            if (dtos is not null)
            {
                List<UserDto> users = (List<UserDto>)dtos;

                var userDto = users.SingleOrDefault(u => u.Id == id);

                if (userDto is null)
                {
                    throw new UserNotFoundExceptions();
                }

                return userDto;
            }


            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);

            await Set();

            _memoryCacheManager.GetOrUpdateDto(Key,user);

            return user.ParseToDto();
        }

        public async Task<string> Register(CreateUserModel model)
        {
            await CheckForExistence(model.Username);

            var user = new User()
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Username = model.Username,
                Gender = GetGender(model.Gender),
                Role = UserConstants.UserRole


            };

            if (user.Username=="super-admin")
            {
                user.Role = UserConstants.AdminRole;
            }


            var passwordHash = new PasswordHasher<User>().HashPassword(user, model.Password);

            user.PasswordHash=passwordHash;

            _unitOfWork.UserRepository.AddUser(user);

            return "Registered successfully!!!";
        }


        public async Task<string> Login(LoginUserModel model)
        {
            var user =await _unitOfWork.UserRepository.GetUserByUsernameAsync(model.UserName);

            if (user == null)
            {
                throw new Exception("User is invalid");
            }

            var result = new PasswordHasher<User>().VerifyHashedPassword(user,user.PasswordHash,model.Password);
             
            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid Password");
            }

            if (string.IsNullOrEmpty(user.Role))
            {
                user.Role = UserConstants.UserRole;
                await _unitOfWork.UserRepository.UpdateUser(user);
            }


            return _jwtManager.GenerateToken(user);

        }



        public async Task<byte[]> AddOrUpdatePhoto(Guid userId,IFormFile file)
        {

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);


            StaticHelper.IsPhoto(file);

            var data = StaticHelper.GetData(file);

            user.PhotoData = data;

            await _unitOfWork.UserRepository.UpdateUser(user);

            await Set();

            return data;

        }



        private async Task CheckForExistence(string username)
        {
            var user =await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

            if (user != null)
            {
                throw new UserExistException();
            }
        }

        private string GetGender(string? gender)
        {

            if (gender == null)
            {
                return UserConstants.Male; 
            }

            bool checkingForGenderExist = gender.ToLower() == UserConstants.Female.ToLower() ||
                                          gender.ToLower() == UserConstants.Male.ToLower();

            if (checkingForGenderExist)
            {
                return gender;
            }

            return UserConstants.Male;
        }


        public async Task<UserDto> UpdateBio(Guid userId,string bio)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);

            if (!string.IsNullOrEmpty(bio))
            {
                user.Bio = bio;
                await _unitOfWork.UserRepository.UpdateUser(user);
            }


            await Set();
            

            return user.ParseToDto();
        }



        public async Task<UserDto> UpdateUserGeneralInfo(Guid userId,UpdateUserGeneralInfo model)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);

            bool check = false;


            if (!string.IsNullOrEmpty(model.Firstname))
            {
                check=true;
                user.Firstname=model.Firstname;
            }

            if (!string.IsNullOrEmpty(model.Lastname))
            {
                check=true;
                user.Lastname = model.Lastname;
            }

            if (!string.IsNullOrEmpty(model.Age))
            {
                try
                {
                    byte age = byte.Parse(model.Age);

                    user.Age = age;

                    check=true;
                }
                catch (Exception e)
                {
                    throw new Exception("Age should be number!!!");
                }
            }

            if (check)
            {
                await _unitOfWork.UserRepository.UpdateUser(user);
                await Set();
            }

            return user.ParseToDto();
        }




        public async Task<UserDto> UpdateUsername(Guid userId, UpdateUsernameModel model)
        {

            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);


            await CheckForExistence(model.Username);

            user.Username= model.Username;

            await _unitOfWork.UserRepository.UpdateUser(user);

            await Set();

            return user.ParseToDto();
        }


        private async Task Set()
        {
            var users = await _unitOfWork.UserRepository.GetAllUsersAsync();

            _memoryCacheManager.GetOrUpdateDto(Key, users.ParseToDtos() );

        }
    }
}
