//-----------------------------------------------------------------------
// <copyright file="EncryptionHelper.cs" company="Microsoft">
//     Copyright © Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>EncryptionHelper class</summary>
//-----------------------------------------------------------------------

using System.Globalization;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using BT.TS360Constants;

namespace BT.TS360API.Common.Helpers
{
    /// <summary>
    /// Encryption Helper class (using AesCryptoServiceProvider)
    /// </summary>
    public static class APIEncryptionHelper
    {        
        private static AesCryptoServiceProvider _provider;

        /// <summary>
        /// Encrypts the value.
        /// </summary>
        /// <param name="inputValue">The value to encrypt.</param>
        /// <returns>The encrypted value.</returns>
        public static string Encrypt(string inputValue)
        {
            if (_provider == null)
            {
                InitProvider();
            }

            var ms = new MemoryStream();
            try
            {
                using (var cryto = new CryptoStream(ms, _provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] valueToEncrypt = new ASCIIEncoding().GetBytes(inputValue);
                    cryto.Write(valueToEncrypt, 0, valueToEncrypt.Length);
                    cryto.FlushFinalBlock();
                    byte[] returnValue = ms.ToArray();

                    return Convert.ToBase64String(returnValue);
                }
            }
            finally 
            {
                ms.Dispose();
            }
        }

        /// <summary>
        /// Decrypts the value.
        /// </summary>
        /// <param name="encryptedValue">The encrypted value.</param>
        /// <returns>The decrypted value.</returns>
        public static string Decrypt(string encryptedValue)
        {
            try
            {
                if (_provider == null)
                {
                    InitProvider();
                }

                byte[] valueToDecrypt;
                try
                {
                    valueToDecrypt = Convert.FromBase64String(encryptedValue);
                }
                catch (FormatException)    
                {
                    // Replace white-space char by '+'
                    encryptedValue = encryptedValue.Trim().Replace(' ', '+');

                    // padding right of '=' char if not multiple of 4
                    if (encryptedValue.Length % 4 > 0)
                        encryptedValue = encryptedValue.PadRight(encryptedValue.Length + 4 - encryptedValue.Length % 4, '=');

                    try
                    {
                        // try again
                        valueToDecrypt = Convert.FromBase64String(encryptedValue);
                    }
                    catch (FormatException ex)
                    {
                        // Write to Elmah
                        var errorMsg = string.Format("Decrypt failed. {0}. Input value is: '{1}'", ex.Message, encryptedValue);
                        var formatExMsg = new FormatException(errorMsg, ex);
                        Logging.Logger.LogException(formatExMsg);

                        return String.Empty;
                    }
                }

                var ms = new MemoryStream();
                try
                {
                    using (var crypto = new CryptoStream(ms, _provider.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        crypto.Write(valueToDecrypt, 0, valueToDecrypt.Length);
                        crypto.FlushFinalBlock();
                        return new UTF8Encoding().GetString(ms.ToArray());
                    }
                }
                finally
                {
                    ms.Dispose();
                }
            }
            catch (CryptographicException cryptoEx)
            {
                Logging.Logger.LogException(cryptoEx);
                return String.Empty;
            }
            catch (Exception ex)
            {
                Logging.Logger.LogException(ex);
                return String.Empty;
            }
        }


        /// <summary>
        /// Create a new Key/IV (this is used by Feature Activation)
        /// </summary>
        /// <returns>An array containing the IV and Key</returns>
        public static string[] GenerateNewKey()
        {
            var result = new string[2];
            _provider = new AesCryptoServiceProvider();
            _provider.GenerateIV();
            _provider.GenerateKey();
            result[0] = Convert.ToBase64String(_provider.IV);
            result[1] = Convert.ToBase64String(_provider.Key);
            return result;
        }

        /// <summary>
        /// Inits the provider.
        /// </summary>
        private static void InitProvider()
        {
            _provider = new AesCryptoServiceProvider();            
            _provider.IV = Convert.FromBase64String(ConfigurationManager.AppSettings["Encryption.IV"]);
            _provider.Key = Convert.FromBase64String(ConfigurationManager.AppSettings["Encryption.Key"]);
        }


        // Hashing algorithms used to verify one-way-hashed passwords:
        // MD5 is used for backward compatibility with Commerce Server 2002. If you have no legacy data, MD5 can be removed.
        // SHA256 is used on Windows Server 2003.
        // SHA1 should be used on Windows XP (SHA256 is not supported).

        private const int SaltValueSize = 4;
        private static readonly string[] HashingAlgorithms = new[] { "SHA256", "MD5" };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="profilePassword"></param>
        /// <returns></returns>
        public static bool VerifyHashedPassword(string password, string profilePassword)
        {
            const int saltLength = SaltValueSize * UnicodeEncoding.CharSize;
            //
            if (String.IsNullOrEmpty(profilePassword) || String.IsNullOrEmpty(password) || profilePassword.Length < saltLength)
            {
                return false;
            }
            // Strip the salt value off the front of the stored password.
            string saltValue = profilePassword.Substring(0, saltLength);

            foreach (string hashingAlgorithmName in HashingAlgorithms)
            {
                HashAlgorithm hash = HashAlgorithm.Create(hashingAlgorithmName);
                string hashedPassword = HashPassword(password, saltValue, hash);
                if (profilePassword.Equals(hashedPassword, StringComparison.Ordinal))
                    return true;
            }
            // None of the hashing algorithms could verify the password.
            return false;
        }

        private static string HashPassword(string clearData, string saltValue, HashAlgorithm hash)
        {
            var encoding = new UnicodeEncoding();
            if (clearData != null && hash != null)
            {
                // If the salt string is null or the length is invalid then
                // create a new valid salt value.
                if (saltValue == null)
                {
                    // Generate a salt string.
                    saltValue = GenerateSaltValue();
                }
                // Convert the salt string and the password string to a single
                // array of bytes. Note that the password string is Unicode and
                // therefore may or may not have a zero in every other byte.
                var binarySaltValue = new byte[SaltValueSize];
                binarySaltValue[0] = Byte.Parse(saltValue.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[1] = Byte.Parse(saltValue.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[2] = Byte.Parse(saltValue.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[3] = Byte.Parse(saltValue.Substring(6, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                var valueToHash = new byte[SaltValueSize + encoding.GetByteCount(clearData)];
                var binaryPassword = encoding.GetBytes(clearData);
                // Copy the salt value and the password to the hash buffer.
                binarySaltValue.CopyTo(valueToHash, 0);
                binaryPassword.CopyTo(valueToHash, SaltValueSize);
                byte[] hashValue = hash.ComputeHash(valueToHash);
                // The hashed password is the salt plus the hash value (as a string).
                var hashedPassword = saltValue;
                foreach (var hexdigit in hashValue)
                {
                    hashedPassword += hexdigit.ToString("X2", CultureInfo.InvariantCulture.NumberFormat);
                }
                // Return the hashed password as a string.
                return hashedPassword;
            }
            return null;
        }

        private static string GenerateSaltValue()
        {
            var utf16 = new UnicodeEncoding();
            // Create a random number object seeded from the value
            // of the last random seed value. This is done
            // interlocked because it is a static value and we want
            // it to roll forward safely.
            var random = new Random(unchecked((int) DateTime.Now.Ticks));
            // Create an array of random values.
            var saltValue = new byte[SaltValueSize];
            random.NextBytes(saltValue);
            // Convert the salt value to a string. Note that the resulting string
            // will still be an array of binary values and not a printable string. 
            // Also it does not convert each byte to a double byte.
            string saltValueString = utf16.GetString(saltValue);
            // Return the salt value as a string.
            return saltValueString;
        }

        /// <summary>
        /// Generate password
        /// </summary>        
        /// <param name="passwordLength">Password length</param>
        /// <returns></returns>
        public static string CreateRandomPassword(int passwordLength)
        {
            var length = passwordLength > 5 ? passwordLength : 6;
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var characters = new char[length];
            var rd = new Random();
            for (var i = 0; i < length; i++)
            {
                characters[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(characters);
        }
    }
}
