using Scrypt;
using System;
using System.IO;
using System.Text;

namespace SynUtil.Crypto
{
    public static class Hash
    {
        public static string MD5(string toHash)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(toHash);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static string MD5(byte[] toHash)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(toHash);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static string SHA1(string toHash)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(toHash);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static string SHA256(string toHash)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(toHash);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        public static string SHA512(string toHash)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.SHA512 sha512 = System.Security.Cryptography.SHA512.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(toHash);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// This hash function will auto generate a salt. Each call to the function will result in a different value. Need to use the BCryptHashIsMatch Method
        /// </summary>
        /// <param name="toHash"></param>
        /// <returns></returns>
        public static string BCryptHash(string toHash)
        {
            string rtn = String.Empty;

            rtn = BCrypt.Net.BCrypt.HashPassword(toHash);

            return rtn;
        }
        public static bool BCryptHashIsMatch(string plainText, string inHash)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, inHash);
        }
        /// <summary>
        /// This hash function will auto generate a salt. Each call to the function will result in a different value. Need to use the ScryptIsMatch Method
        /// </summary>
        /// <param name="toHash"></param>
        /// <returns></returns>
        public static string Scrypt(string toHash)
        {
            ScryptEncoder encod = new ScryptEncoder();

            string hash = encod.Encode(toHash);

            return hash;
        }
        public static bool ScryptIsMatch(string plainText, string inHash)
        {
            ScryptEncoder encod = new ScryptEncoder();

            return encod.Compare(plainText, inHash);
        }
        public static string LoopedSHA512(string toHash)
        {
            //Default number of loops so that the hash takes about 1/10th of a second to calculate
            //With a string of 8 characters [a-z,A-Z,0-9] there are 2.18e14 possible combinations
            //That results in a single CPU taking 2.18e13 seconds (~692,352 years) to build a complete rainbow table
            //This assumes no shortcut is possible
            return LoopedSHA512(toHash, 7500);
        }
        /// <summary>
        /// This should not be used for password hashing since Scrypt or BCrypt work better for that
        /// This function is intended to provide some security by obscurity on top of a proven hash algorithm SHA512
        /// By working in some unorthodox string manipulation we will avoid any existing rainbow tables
        /// Given enough loops it would make building a new rainbow table unreasonable
        /// Brute force attempts would be also unreasonable given enough loops
        /// It might be possible to run a parallel cracking tool that could circumvent the looping
        /// </summary>
        /// <param name="toHash"></param>
        /// <param name="loops"></param>
        /// <returns></returns>
        public static string LoopedSHA512(string toHash, int loops)
        {
            string loopedHash = toHash;

            if(String.IsNullOrEmpty(toHash))
                throw new Exception("Value to loop hash cannot be blank");
            if (loops == 0)
                throw new Exception("Loop count cannot be 0");

            for (int i = 0; i < loops; i++)
            {
                if (i == 0)
                {
                    loopedHash = SHA512(loopedHash);
                }
                else if (i % 3 == 0)
                {
                    loopedHash = SHA512(loopedHash + MD5(toHash));
                }
                else if (i % 4 == 0)
                {
                    loopedHash = SHA512(loopedHash + SHA256(toHash).Substring(3, 7));
                }
                else
                {
                    loopedHash = SHA512(loopedHash + toHash.Substring(0,1));
                }
            }

            return loopedHash;
        }
        public static string BytesToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
        public static string CalculateMD5Checksum(string fileName)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    byte[] hash = md5.ComputeHash(stream);

                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
