using System.Security;
using System.Text;

namespace Braco.Services.Abstractions
{
    /// <summary>
    /// Deals with security such as logging in.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Encrypts a plain string.
        /// </summary>
        /// <param name="plainText">String to encrypt.</param>
        /// <param name="password">Password to use for encryption.</param>
        /// <returns>Encrypted text.</returns>
        string Encrypt(string plainText, string password);

        /// <summary>
        /// Decrypts an already encrypted text.
        /// </summary>
        /// <param name="encryptedText">String to decrypt.</param>
        /// <param name="password">Password to use for decryption.</param>
        /// <returns>Plain text.</returns>
        string Decrypt(string encryptedText, string password);

        /// <summary>
        /// Secures a plain string using a secure hashing algorithm.
        /// </summary>
        /// <param name="plainText">String to hash.</param>
        /// <returns>Secure hashed string.</returns>
        string Hash(string plainText);

        /// <summary>
        /// Hashes a secure string using a secure hashing algorithm.
        /// </summary>
        /// <param name="secureString">Secure string to hash.</param>
        /// <returns>Secure hashed string.</returns>
        string Hash(SecureString secureString);

        /// <summary>
        /// Checks if the given text matches the given hash.
        /// <para>Common use case: verifying password.</para>
        /// </summary>
        /// <param name="plainText">String to check against the hash.</param>
        /// <param name="hashToCheck">Hash used for comparing with the given string.</param>
        /// <returns>True if the text matches the hash. Otherwise false.</returns>
        bool MatchesHash(string plainText, string hashToCheck);

        /// <summary>
        /// Checks if the given text matches the given hash.
        /// <para>Common use case: verifying password.</para>
        /// </summary>
        /// <param name="secureString">Secure string to check against the hash.</param>
        /// <param name="hashToCheck">Hash used for comparing with the given string.</param>
        /// <returns>True if the text matches the hash. Otherwise false.</returns>
        bool MatchesHash(SecureString secureString, string hashToCheck);

        /// <summary>
        /// Generates a random byte array with cryptographically
        /// secure random bytes.
        /// </summary>
        /// <param name="count">Number of bytes to generate. Example:
        /// 32 bytes will produce 256 bits.</param>
        /// <returns>Secure random byte array.</returns>
        byte[] GenerateSecureRandomByteArray(int count);
    }
}
