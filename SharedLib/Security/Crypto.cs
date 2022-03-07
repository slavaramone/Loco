using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace SharedLib.Security
{
    public static class Crypto
    {
        private const int IterationsCount = 10000;
        private const int NumBytesRequested = 256 / 8;

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string HashPassword(string password, string salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: password,
                            salt: Convert.FromBase64String(salt),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: IterationsCount,
                            numBytesRequested: NumBytesRequested));
        }
    }
}
