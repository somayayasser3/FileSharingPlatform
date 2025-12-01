using System.Security.Cryptography;

namespace FileSharingPlatform.Helpers
{
    public class ShareLinkHelper
    {
        public static string GenerateSecureToken(int length = 32)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            random.GetBytes(bytes);

            return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
        }
    }
}
