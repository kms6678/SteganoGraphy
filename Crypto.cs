using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.IO;

namespace steganography
{
    class Crypto
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("123456789abcdefghijkllmn");

        public static string EncryptStringAES(string HiddenText, string PrivateKey)
        {
            string outStr = null;
            RijndaelManaged AES = null;

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(PrivateKey, _salt);

                AES = new RijndaelManaged();
                AES.Key = key.GetBytes(AES.KeySize / 8);

                ICryptoTransform encryptor = AES.CreateEncryptor(AES.Key, AES.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(BitConverter.GetBytes(AES.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(AES.IV, 0, AES.IV.Length);

                    byte[] temp = msEncrypt.ToArray();

                    outStr = Convert.ToBase64String(msEncrypt.ToArray());

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(HiddenText);
                        }
                    }
                    temp = msEncrypt.ToArray();

                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }//사용법 MSDN 참조
            finally
            {
                if (AES != null)
                    AES.Clear();
            }

            return outStr;
        }

        public static string DecryptStringAES(string ExtractedText, string PrivateKey)
        {
            RijndaelManaged AES = null;

            string plaintext = null;

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(PrivateKey, _salt);

                ExtractedText.Replace(" ", "+");
                byte[] bytes = Convert.FromBase64String(ExtractedText);

                using (MemoryStream msDncrypt = new MemoryStream(bytes))
                {
                    AES = new RijndaelManaged();
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = Read_IV(msDncrypt);
                    ICryptoTransform decryptor = AES.CreateDecryptor(AES.Key, AES.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }//사용법 MSDN참조
            finally
            {
                if (AES != null)
                {
                    AES.Clear();
                }
            }

            return plaintext;
        }

        private static byte[] Read_IV(Stream streamstr)
        {
            byte[] IVlength = new byte[sizeof(int)];
            int count = streamstr.Read(IVlength, 0, IVlength.Length);

            if (count != IVlength.Length)
            {
                throw new SystemException();
            }

            byte[] buffer = new byte[BitConverter.ToInt32(IVlength, 0)];

            int buffercount = streamstr.Read(buffer, 0, buffer.Length);
            if (buffercount != buffer.Length)
            {
                throw new SystemException();
            }

            return buffer;
        }
    }
}
