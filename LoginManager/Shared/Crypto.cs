using System.Security.Cryptography;

namespace LoginManager.Shared
{
    public static class Crypto
    {
        private static string keyFullPathName = PlatformSpecifics.GetPathToCryptoKeySetting();
        private static string ivFullPathName = PlatformSpecifics.GetPathToCryptoIVSetting();

        
        public static byte[] GetKey() 
        {
            if (!File.Exists(keyFullPathName))
                throw new Exception("Key file not found");
                
            return File.ReadAllBytes(keyFullPathName).Take<byte>(16).ToArray();
        }

        public static byte[] GetIV()
        {
            if (!File.Exists(ivFullPathName))
                throw new Exception("IV file not found");

            return File.ReadAllBytes(ivFullPathName).Take<byte>(16).ToArray();
        }

        /*
        public static bool Create()
        {
            byte[] byk = new byte[16];
            byte[] byiv = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                byk = aes.Key;
                aes.GenerateIV();
                byiv = aes.IV;

            }
            File.WriteAllBytes(ivFullPathName, byiv);
            File.WriteAllBytes(keyFullPathName, byk);
            return true;
        }
        */


        public static string Encrypt(string plainText)
        {
            // Create a new AesManaged.
            // It takes a byte array of 16, 24, or 32 bytes.
            /*
            byte[] key = Encoding.UTF8.GetBytes("0123456789abcdef");
            byte[] iv = Encoding.UTF8.GetBytes("fedcba9876543210");
            */
            return EncryptString(plainText, GetKey(), GetIV());
        }

        public static int NumberOfKeyBytes()
        {
            return GetKey().Length;
        }

        public static int NumberOfIVBytes()
        {
            return GetIV().Length;
        }

        public static string Decrypt(string cipherText)
        {
            if (!File.Exists(ivFullPathName))
                throw new Exception("IV file not found");

            byte[] iv = File.ReadAllBytes(ivFullPathName);
            return DecryptString(cipherText, GetKey(), GetIV()); 
        }

        public static int TestOfTest()
        {
            return 42;
        }

        private static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                using (MemoryStream msDecrypt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swDecrypt = new StreamReader(csDecrypt))
                        {
                           return swDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        private static string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }
}