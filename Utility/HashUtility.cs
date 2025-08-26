using System.Buffers.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using AdvancedREI;

namespace Back_Almazara.Utility
{
    public class HashUtility : IHashUtility
    {
        private readonly long _secretKey;
        public HashUtility(IConfiguration configuration)
        {
            _secretKey = long.Parse(configuration["SecretKeyEncryptId"]);
        }


        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convertir la cadena a bytes
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convertir bytes a string hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }



        public string ToBase36(long num)
        {
            long obfuscated = num ^ _secretKey;
            return Base36.NumberToBase36(obfuscated);
        }

        public long FromBase36(string obfuscated)
        {
            long decodedValue = Base36.Base36ToNumber(obfuscated.ToUpper());
            return decodedValue ^ _secretKey;
            //return int.Parse(obfuscated);
        }
    }
}
