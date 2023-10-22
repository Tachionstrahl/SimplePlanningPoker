using System;
using System.Net.NetworkInformation;
using System.Text;

namespace SimplePlanningPoker.Utils
{

    public static class RandomIDGenerator
    {
        private const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static readonly Random random = new();

        public static string GenerateRandomID(int length)
        {
            StringBuilder sb = new(length);

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(AllowedChars.Length);
                sb.Append(AllowedChars[index]);
            }

            return sb.ToString();
        }
    }
}

