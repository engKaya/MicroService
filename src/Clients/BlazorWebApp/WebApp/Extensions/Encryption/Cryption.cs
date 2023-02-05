using System;
using System.Text;

namespace WebApp.Extensions.Encryption
{
    public static class Cryption
    {
        public static string GetBase64String(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string GetTextFromBase64(string base64)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }


    }
}
