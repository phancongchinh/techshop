using System.Security.Cryptography;
using System.Text;

namespace Techshop.Services;

public class PasswordConverter
{
    public static string Hash(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = MD5.HashData(bytes);
        return Encoding.UTF8.GetString(hash);
    }

    public static bool Match(string password, string hash)
    {
        var hashPass = Hash(password);
        var hashToBytes = Encoding.UTF8.GetBytes(hash);
        var passToBytes = Encoding.UTF8.GetBytes(hashPass);
        var match = true;
        for (var i = 0; i < hashToBytes.Length; i++)
        {
            if (hashToBytes[i] != passToBytes[i])
            {
                match = false;
            }
        }

        return match;
    }
}