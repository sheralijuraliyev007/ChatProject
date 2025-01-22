using Chat.Api.Exceptions;
using Chat.Api.Helpers;
using Chat.Api.Managers;
using Chat.Api.Models.UserModels;
using Chat.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserManager userManager, UserHelper userHelper) : ControllerBase
    {
        private readonly UserHelper _userHelper = userHelper;
        private readonly UserManager _userManager = userManager;
        private Guid UserId => _userHelper.GetUserId();

        [HttpGet]
        [Authorize(/*Roles = "admin, user"*/)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.GetAllUsers();
            return Ok(users);
        }


        [Authorize(/*Roles = "admin, user"*/)]
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserById()
        {
            try
            {
                var user =await _userManager.GetUserById(UserId);


                return Ok(user);

            }
            catch (UserNotFoundExceptions e)
            {
                return NotFound();
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]CreateUserModel model)
        {
            var result = await _userManager.Register(model);

            return Ok(result);

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserModel model)
        {
            try
            {
                var result = await _userManager.Login(model);


                return Ok(result);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(/*Roles = "admin, user"*/)]
        [HttpPost("add-or-update-photo")]
        public async Task<IActionResult> AddOrUpdateUserPhoto([FromForm] PhotoClass photo)
        {
            var result = await _userManager.AddOrUpdatePhoto(UserId,photo.File);

            return Ok(result);
        }

        [Authorize(/*Roles = "admin, user"*/)]
        [HttpPost("update-bio")]
        public async Task<IActionResult> UpdateBio([FromBody] string bio)
        {
            var result = await _userManager.UpdateBio(UserId, bio);

            return Ok(result);
        }


        [Authorize(/*Roles = "admin, user"*/)]
        [HttpPost("update-user-general-info")]
        public async Task<IActionResult> UpdateUserGeneralInfo([FromBody] UpdateUserGeneralInfo info)
        {
            try
            {
                var result = await _userManager.UpdateUserGeneralInfo(UserId, info);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [Authorize(/*Roles = "admin, user"*/)]
        [HttpPost("update-username")]
        public async Task<IActionResult> UpdateUsername([FromBody] UpdateUsernameModel model)
        {
            try
            {
                var result = await _userManager.UpdateUsername(UserId,model);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



    }


    public class PhotoClass
    {
        public string? Name { get; set; }
        

        public string? Description { get; set; }

        public IFormFile File{ get; set; }
    }
}
