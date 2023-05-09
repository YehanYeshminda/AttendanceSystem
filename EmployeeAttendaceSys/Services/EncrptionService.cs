using System.Security.Cryptography;
using System.Text;

namespace EmployeeAttendaceSys.Services
{
    public class EncrptionService
    {
        private TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
        private const string passphrase = "ABCD";

        public EncrptionService() { }

        public string EncryptData(string Message)
        {
            byte[] Results;
            UTF8Encoding UTF8 = new UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
            TripleDes.Key = TDESKey;
            TripleDes.Mode = CipherMode.ECB;
            TripleDes.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(Message);
            try
            {
                ICryptoTransform Encryptor = TripleDes.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TripleDes.Clear();
                HashProvider.Clear();
            }

            return Convert.ToBase64String(Results);
        }

        public string DecryptString(string Message)
        {
            byte[] Results;
            UTF8Encoding UTF8 = new UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));
            TripleDes.Key = TDESKey;
            TripleDes.Mode = CipherMode.ECB;
            TripleDes.Padding = PaddingMode.PKCS7;
            byte[] DataToDecrypt = Convert.FromBase64String(Message);
            try
            {
                ICryptoTransform Decryptor = TripleDes.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                TripleDes.Clear();
                HashProvider.Clear();
            }
            return UTF8.GetString(Results);
        }
    }
    }
