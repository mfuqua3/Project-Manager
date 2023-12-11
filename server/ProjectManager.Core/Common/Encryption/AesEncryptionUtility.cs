using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProjectManager.Common.Encryption;

/// <summary>
/// Provides methods for encrypting and decrypting text using AES encryption algorithm.
/// </summary>
public static class AesEncryptionUtility
{
    /// <summary>
    /// This constant variable represents the number of iterations used in a process.
    /// </summary>
    private const int Iterations = 10000; // you may increase this number to make it more secure

    /// <summary>
    /// The size of the salt used for cryptographic operations.
    /// </summary>
    private const int SaltSize = 16;

    /// <summary>
    /// Encrypts the given text using the provided key.
    /// </summary>
    /// <param name="text">The text to be encrypted.</param>
    /// <param name="key">The key used for encryption.</param>
    /// <returns>The encrypted text as a Base64 encoded string.</returns>
    public static string Encrypt(string text, string key)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Text must not be null or empty", nameof(text));
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key must not be null or empty", nameof(key));

        var bytes = Encoding.Unicode.GetBytes(text);
        using var aes = Aes.Create();
        var salt = GenerateSalt();

        var hashAlgorithmName = HashAlgorithmName.SHA256; 
         
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(key, salt, Iterations, hashAlgorithmName);


        aes.Mode = CipherMode.CBC;
        aes.Key = rfc2898DeriveBytes.GetBytes(32);
        aes.IV = rfc2898DeriveBytes.GetBytes(16);

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        cryptoStream.Write(bytes, 0, bytes.Length);
        cryptoStream.FlushFinalBlock();

        return Convert.ToBase64String(salt.Concat(memoryStream.ToArray()).ToArray());
    }

    /// <summary>
    /// Decrypts the given encrypted string using the specified key.
    /// </summary>
    /// <param name="encryptedString">The encrypted string to be decrypted.</param>
    /// <param name="key">The key used for decryption.</param>
    /// <returns>The decrypted string.</returns>
    public static string Decrypt(string encryptedString, string key)
    {
        if (string.IsNullOrEmpty(encryptedString))
            throw new ArgumentNullException(nameof(encryptedString), "Source string is null");
        if (string.IsNullOrEmpty(key))
            throw new ArgumentNullException(nameof(key), "Source key is null");

        var source = Convert.FromBase64String(encryptedString);
        var salt = source.Take(SaltSize).ToArray();
        var encryptedBytes = source.Skip(SaltSize).ToArray();

        using var aes = Aes.Create();

        var hashAlgorithmName = HashAlgorithmName.SHA256;
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(key, salt, Iterations, hashAlgorithmName);

        aes.Mode = CipherMode.CBC;
        aes.Key = rfc2898DeriveBytes.GetBytes(32);
        aes.IV = rfc2898DeriveBytes.GetBytes(16);

        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
        cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
        cryptoStream.FlushFinalBlock();

        return Encoding.Unicode.GetString(memoryStream.ToArray());
    }

    /// <summary>
    /// Generates a random salt of the specified size.
    /// </summary>
    /// <returns>A byte array containing the generated salt.</returns>
    private static byte[] GenerateSalt()
    {
        var data = new byte[SaltSize];
        RandomNumberGenerator.Fill(data);
        return data;
    }
}