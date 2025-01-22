using Chat.Api.Constants;
using Chat.Api.Exceptions;
using MemoryStream = System.IO.MemoryStream;

namespace Chat.Api.Helpers
{
    public static class StaticHelper
    {
        public static string GetFullName(string firstName, string lastName)
        {
            return $"{firstName} {lastName}";
        }

        public static void IsPhoto(IFormFile file)
        {
            var type = file.ContentType;



            var check=type== UserConstants.JpgType || type == UserConstants.PngType;

            if (!check)
            {
                throw new NotPhotoType();
            }
        }

        public static byte[] GetData(IFormFile file)
        {
            var ms = new MemoryStream();

            file.CopyTo(ms);

            return ms.ToArray();
        }
    }
}
