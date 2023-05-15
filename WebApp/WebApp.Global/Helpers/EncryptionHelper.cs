using System.Security.Cryptography;
using System.Text;
using WebApp.Global.Constants;

namespace WebApp.Global.Helpers
{
    public static class EncryptionHelper
    {
        #region Public Methods
        //To encypt the data
        public static string Encrypt(string key, string data)
        {
            string encryptData = null;
            byte[][] keys = GetHashKeys(key);
            try
            {
                encryptData = EncryptStringToBytes(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentException) { }

            return encryptData;
        }

        //To decrypt the data
        public static string Decrypt(string key, string data)
        {
            string decryptData = null;
            byte[][] keys = GetHashKeys(key);

            try
            {
                decryptData = DecryptBytesToString(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            return decryptData;
        }
        #endregion

        #region Private Helper Methods
        private static string EncryptStringToBytes(string data, byte[] key, byte[] IV)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException("data");
            }

            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (IV == null)
            {
                throw new ArgumentNullException("IV");
            }

            byte[] encrypted;

            using (AesManaged aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = IV;

                ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(data);
                        }

                        encrypted = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }
        private static string DecryptBytesToString(string data, byte[] key, byte[] IV)
        {
            try
            {
                byte[] dataBytes = Convert.FromBase64String(data);

                if (dataBytes == null)
                {
                    throw new ArgumentNullException("data");
                }

                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                if (IV == null)
                {
                    throw new ArgumentNullException("IV");
                }

                string decrypted = string.Empty;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = IV;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream ms = new MemoryStream(dataBytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                decrypted = sr.ReadToEnd();
                            }
                        }
                    }
                }

                return decrypted;
            }
            catch (FormatException)
            {
                return ResponseStaticMessages.RESET_TOKEN_INVALID;
            }
        }
        private static byte[][] GetHashKeys(string key)
        {
            byte[][] keys = new byte[2][];

            Encoding enc = Encoding.UTF8;
            SHA256 sha256 = new SHA256CryptoServiceProvider();

            byte[] rawKey = enc.GetBytes(key);
            byte[] rawIV = enc.GetBytes(key);

            byte[] hashKey = sha256.ComputeHash(rawKey);
            byte[] hashIV = sha256.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            keys[0] = hashKey;
            keys[1] = hashIV;

            return keys;

        }
        #endregion
    }
}
