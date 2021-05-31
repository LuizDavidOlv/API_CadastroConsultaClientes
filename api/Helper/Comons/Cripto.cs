using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Helper.Comons
{
    public class Cripto
    {
        #region Enum
        /// <summary>
        /// Niveis de criptografia AES
        /// Criptografia de 128, 192 e 256 Bits.
        /// </summary>
        private enum AESCryptographyLevel : int
        {
            /// <summary>
            /// Criptografia AES 128 Bits
            /// </summary>
            AES_128 = 128,

            /// <summary>
            /// Criptografia AES 192 Bits
            /// </summary>
            AES_192 = 192,

            /// <summary>
            /// Criptografia AES 256 Bits
            /// </summary>
            AES_256 = 256
        }
        #endregion

        private static string password = "Senha@Cencosud*";
        private static AESCryptographyLevel bits = AESCryptographyLevel.AES_128;

        public static string Decrypt(string data)
        {
            byte[] cipherBytes = Convert.FromBase64String(data);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, new byte[] { 0x01, 0x04, 0x02, 0x1D, 0x03, 0x1E, 0x04, 0x1C, 0x06, 0x21, 0xD1, 0xAD, 0xAF, 0xA4, 0x0F });

            if (bits == AESCryptographyLevel.AES_128)
            {
                byte[] decryptedData = _Decrypt(cipherBytes, pdb.GetBytes(16), pdb.GetBytes(16));
                return Encoding.Unicode.GetString(decryptedData);
            }
            else if (bits == AESCryptographyLevel.AES_192)
            {
                byte[] decryptedData = _Decrypt(cipherBytes, pdb.GetBytes(24), pdb.GetBytes(16));
                return Encoding.Unicode.GetString(decryptedData);
            }
            else if (bits == AESCryptographyLevel.AES_256)
            {
                byte[] decryptedData = _Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
                return Encoding.Unicode.GetString(decryptedData);
            }
            else
            {
                return string.Concat(bits);
            }
        }

        private static byte[] _Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                Rijndael alg = Rijndael.Create();
                alg.Key = Key;
                alg.IV = IV;
                CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(cipherData, 0, cipherData.Length);
                cs.Close();
                byte[] decryptedData = ms.ToArray();
                return decryptedData;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
