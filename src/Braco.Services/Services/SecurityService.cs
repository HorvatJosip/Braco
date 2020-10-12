using Braco.Services.Abstractions;
using Braco.Utilities.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace Braco.Services
{
    // Encryption and decryption logic: https://stackoverflow.com/a/10177020
    /// <summary>
    /// <see cref="ISecurityService"/> implementation.
    /// Handles encryption, decryption and hashing.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int KeySize = 128;
        private const int NumBytes = KeySize / 8;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

		/// <summary>
		/// Format identifier for hexadecimal characters.
		/// </summary>
        public const string HexStringFormat = "X2";

		/// <inheritdoc/>
        public string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = GenerateSecureRandomByteArray(NumBytes);
            var ivStringBytes = GenerateSecureRandomByteArray(NumBytes);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations);
            var keyBytes = password.GetBytes(NumBytes);
            using var symmetricKey = new RijndaelManaged
            {
                BlockSize = KeySize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };

            using var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
            var cipherTextBytes = saltStringBytes;
            cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

		/// <inheritdoc/>
        public string Decrypt(string encryptedText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(encryptedText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(NumBytes).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(NumBytes).Take(NumBytes).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip(NumBytes * 2).Take(cipherTextBytesWithSaltAndIv.Length - (NumBytes * 2)).ToArray();

            using var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations);
            var keyBytes = password.GetBytes(NumBytes);
            using var symmetricKey = new RijndaelManaged
            {
                BlockSize = KeySize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7
            };
            using var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes);
            using var memoryStream = new MemoryStream(cipherTextBytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];
            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

		/// <inheritdoc/>
        public byte[] GenerateSecureRandomByteArray(int count)
        {
            var randomBytes = new byte[count]; // 32 Bytes will give us 256 bits.
            using var rngCsp = new RNGCryptoServiceProvider();
            // Fill the array with cryptographically secure random bytes.
            rngCsp.GetBytes(randomBytes);
            return randomBytes;
        }

		/// <inheritdoc/>
        public string Hash(string plainText)
            => BCrypt.Net.BCrypt.EnhancedHashPassword(plainText);

		/// <inheritdoc/>
        public string Hash(SecureString secureString)
            => Hash(secureString.Unsecure());

		/// <inheritdoc/>
        public bool MatchesHash(string plainText, string hashToCheck)
            => BCrypt.Net.BCrypt.EnhancedVerify(plainText, hashToCheck);

		/// <inheritdoc/>
        public bool MatchesHash(SecureString secureString, string hashToCheck)
            => MatchesHash(secureString.Unsecure(), hashToCheck);
    }
}
