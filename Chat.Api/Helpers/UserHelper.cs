using System.Security.Claims;

namespace Chat.Api.Helpers
{
    public class UserHelper(IHttpContextAccessor contextAccessor)
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;


        public Guid GetUserId()
        {
            var userId = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.Parse(userId!);
        }

        public string GetUserName()
        {
            var userName = _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return userName!;
        }
    }
}
