using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SynUtil.Crypto
{
    public class AesEncryption
    {
        public byte[] EncryptFile(string pathToFile, string encryptionKey)
        {
            FileStream fs = File.Open(pathToFile, FileMode.Open);
            MemoryStream ms = new MemoryStream();

            fs.CopyTo(ms);

            if (fs != null)
                fs.Dispose();

            return EncryptFile(ms, encryptionKey);
        }
        public byte[] EncryptFile(byte[] byteArray, string encryptionKey)
        {
            Stream stream = new MemoryStream(byteArray);

            return EncryptFile(stream, encryptionKey);
        }
        public byte[] EncryptFile(Stream fileStream, string encryptionKey)
        {
            UnicodeEncoding unicodeEncoding = new UnicodeEncoding();

            MemoryStream tempMemStream = new MemoryStream();
            MemoryStream returnStream = new MemoryStream();
            RijndaelManaged crypto = new RijndaelManaged();

            crypto.KeySize = 256;
            crypto.BlockSize = 128;
            crypto.Mode = CipherMode.CBC;

            var key = new Rfc2898DeriveBytes(encryptionKey, unicodeEncoding.GetBytes(encryptionKey.Substring(0, 8)), 1000);
            crypto.Key = key.GetBytes(crypto.KeySize / 8);
            crypto.IV = key.GetBytes(crypto.BlockSize / 8);

            CryptoStream cryptoStream = new CryptoStream(tempMemStream, crypto.CreateEncryptor(), CryptoStreamMode.Write);
            Stream inputFileStream = fileStream;

            inputFileStream.Seek(0, SeekOrigin.Begin);

            int data;
            while ((data = inputFileStream.ReadByte()) != -1)
            {
                cryptoStream.WriteByte((byte)data);
            }
            //Flush the data out so it is fully written to the underlying stream.
            cryptoStream.FlushFinalBlock();

            if (tempMemStream.Length > 0)
            {
                tempMemStream.Position = 0;
                tempMemStream.CopyTo(returnStream); //Copy here so that we can close the CryptoStream otherwise if we do not copy it out then when we close CryptoStream it closes the target stream with it
                returnStream.Position = 0;
            }

            if (cryptoStream != null)
                cryptoStream.Dispose();
            if (inputFileStream != null)
                inputFileStream.Dispose();
            if (tempMemStream != null)
                tempMemStream.Dispose();

            return returnStream.ToArray();
        }

        public byte[] DecryptFile(string pathToFile, string encryptionKey)
        {
            FileStream fs = File.Open(pathToFile, FileMode.Open);
            MemoryStream ms = new MemoryStream();

            fs.CopyTo(ms);

            if (fs != null)
                fs.Dispose();

            return DecryptFile(ms, encryptionKey);
        }
        public byte[] DecryptFile(byte[] byteArray, string encryptionKey)
        {
            Stream stream = new MemoryStream(byteArray);

            return DecryptFile(stream, encryptionKey);
        }
        public byte[] DecryptFile(Stream fileStream, string encryptionKey)
        {
            UnicodeEncoding unicodeEncoding = new UnicodeEncoding();

            MemoryStream returnStream = new MemoryStream();
            RijndaelManaged crypto = new RijndaelManaged();

            crypto.KeySize = 256;
            crypto.BlockSize = 128;
            crypto.Mode = CipherMode.CBC;

            var key = new Rfc2898DeriveBytes(encryptionKey, unicodeEncoding.GetBytes(encryptionKey.Substring(0, 8)), 1000);
            crypto.Key = key.GetBytes(crypto.KeySize / 8);
            crypto.IV = key.GetBytes(crypto.BlockSize / 8);

            fileStream.Seek(0, SeekOrigin.Begin);
            Stream inputFileStream = fileStream;
            CryptoStream cryptoStream = new CryptoStream(inputFileStream, crypto.CreateDecryptor(), CryptoStreamMode.Read);

            int data;
            while ((data = cryptoStream.ReadByte()) != -1)
            {
                returnStream.WriteByte((byte)data);
            }

            if (cryptoStream != null)
                cryptoStream.Dispose();
            if (inputFileStream != null)
                inputFileStream.Dispose();

            return returnStream.ToArray();
        }

        public string Encrypt(string plainText, string password)
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            string keyPT = password;
            string saltString = GetRandomSalt().Substring(0, 24);

            byte[] salt = Encoding.ASCII.GetBytes(saltString);
            saltString = Convert.ToBase64String(salt);

            int myIterations = 1000; //Both encrypt and decrypt need to use the same number of iterations for the encryption/decryption to work. Default to 1,000
            Rfc2898DeriveBytes derivedBytesKey = new Rfc2898DeriveBytes(keyPT, salt, myIterations);

            // Sets correct size (/8)
            myRijndael.Key = derivedBytesKey.GetBytes(myRijndael.KeySize / 8);
            myRijndael.IV = derivedBytesKey.GetBytes(myRijndael.BlockSize / 8);

            // Encrypt the string to an array of bytes.
            byte[] encryptedBytes = EncryptStringToBytesAES(plainText, myRijndael.Key, myRijndael.IV);
            string encryptedBase64 = Convert.ToBase64String(encryptedBytes);

            //***** Key/Salt
            saltString = HexConversion.TextToHex(saltString.Trim());
            //***** Encrypted value
            string encrypted = HexConversion.TextToHex(encryptedBase64.Trim());

            //***** Combine the encrypted text with the key/salt and return
            string encryptedString = encrypted + " " + saltString;

            return encryptedString;
        }
        public string Decrypt(string encryptedText, string password)
        {
            RijndaelManaged myRijndael = new RijndaelManaged();
            string keyPT = password;
            encryptedText = HexConversion.HexToText(encryptedText);
            string salt = encryptedText.Substring(encryptedText.Length - 32); //Since the encryption is a random length, the trailing right 32 characters will always be the salt value
            string encrypted = encryptedText.Substring(0, (encryptedText.Length - 32)).Trim();
            byte[] saltBytes = Convert.FromBase64String(salt);
            int myIterations = 1000; //Both encrypt and decrypt need to use the same number of iterations for the encryption/decryption to work. Default to 1,000
            Rfc2898DeriveBytes derivedBytesKey = new Rfc2898DeriveBytes(keyPT, saltBytes, myIterations);

            // Sets correct size (/8)
            myRijndael.Key = derivedBytesKey.GetBytes(myRijndael.KeySize / 8);
            myRijndael.IV = derivedBytesKey.GetBytes(myRijndael.BlockSize / 8);

            byte[] decryptedBytes = new byte[encrypted.Length];

            decryptedBytes = Convert.FromBase64String(encrypted);
            string decryptedString = DecryptStringFromBytesAES(decryptedBytes, myRijndael.Key, myRijndael.IV);

            return decryptedString;
        }

        private byte[] EncryptStringToBytesAES(string plainText, byte[] key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            MemoryStream msEncrypt = null;
            CryptoStream csEncrypt = null;
            StreamWriter swEncrypt = null;
            RijndaelManaged aesAlg = null;

            try
            {
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                msEncrypt = new MemoryStream();
                csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                swEncrypt = new StreamWriter(csEncrypt);

                swEncrypt.Write(plainText);

            }
            finally
            {
                if (swEncrypt != null)
                    swEncrypt.Close();
                if (csEncrypt != null)
                    csEncrypt.Close();
                if (msEncrypt != null)
                    msEncrypt.Close();
                if (aesAlg != null)
                    aesAlg.Clear();
            }
            return msEncrypt.ToArray();
        }
        private string DecryptStringFromBytesAES(byte[] cipherText, byte[] key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            MemoryStream msDecrypt = null;
            CryptoStream csDecrypt = null;
            StreamReader srDecrypt = null;
            RijndaelManaged aesAlg = null;

            string plaintext = null;

            try
            {
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key; //This will error if the array is not a 32 bit array
                aesAlg.IV = IV; //This will error if the array is not a 16 bit array
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                msDecrypt = new MemoryStream(cipherText);
                csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                srDecrypt = new StreamReader(csDecrypt);

                plaintext = srDecrypt.ReadToEnd(); //Read from stream, this will be plain text
            }
            finally
            {
                if (srDecrypt != null)
                    srDecrypt.Close();
                if (csDecrypt != null)
                    csDecrypt.Close();
                if (msDecrypt != null)
                    msDecrypt.Close();
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        public string GetRandomSalt()
        {
            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[32]; //256 bits

            random.GetBytes(salt);

            return HexConversion.BytesToHex(salt);
        }
    }
}
