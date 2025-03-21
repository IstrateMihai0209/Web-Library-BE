using OnlineLibrary.Models.Book;

namespace Extensions
{
    public static class BookExtensions
    {
        public static string GetFileExtension(this IFormFile file)
        {
            switch (file.ContentType)
            {
                case "application/pdf":
                    return ".pdf";
                case "image/jpeg":
                    return ".jpg";
                default:
                    return string.Empty;
            }
        }
    }
}
