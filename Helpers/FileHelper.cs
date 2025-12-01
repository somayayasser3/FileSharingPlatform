using System.Security.Cryptography;

namespace FileSharingPlatform.Helpers
{
    public class FileHelper
    {
        public static readonly string[] AllowedMimeTypes = new[]
        {
            ".png", ".jpg", ".jpeg", ".pdf", ".xlsx", ".zip", ".rar"
        };
        public const long MaxFileSizeInBytes = 10 * 1024 * 1024; // 10 MB

        public static bool IsValidFileExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return AllowedMimeTypes.Contains(extension);
        }

        public static bool IsValidFileSize(long fileSize)
        {
            return fileSize <= MaxFileSizeInBytes;
        }
    }
}
