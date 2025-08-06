using System.Security.Cryptography;
using System.Text;
using AppServices.Abstract;

namespace AppServices.Concrete
{
    public class EncrypthonService : IEncrypthonService
    {
        private static readonly string Key = "acf5&3vdk49v*dgo";
        private static readonly string IV = "1a3d5f7g9h2j4l6n";

        public  string AesEncrypthon(string text)
        {
            Aes aesAlgorithm = Aes.Create();
            aesAlgorithm.Key = Encoding.UTF8.GetBytes(Key);
            aesAlgorithm.IV = Encoding.UTF8.GetBytes(IV);
            ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(aesAlgorithm.Key, aesAlgorithm.IV);

            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using StreamWriter swEncrypt = new(csEncrypt);
            swEncrypt.Write(text);
            swEncrypt.Close();

            return Convert.ToBase64String(msEncrypt.ToArray());

        }

        public  string AesDecrypthon(string text)
        {
            using Aes aesAlgorithm = Aes.Create();
            aesAlgorithm.Key = Encoding.UTF8.GetBytes(Key);
            aesAlgorithm.IV = Encoding.UTF8.GetBytes(IV);

            ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor(aesAlgorithm.Key, aesAlgorithm.IV);
            byte[] buffer = Convert.FromBase64String(text);

            using MemoryStream msDecrypt = new(buffer);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }
}
