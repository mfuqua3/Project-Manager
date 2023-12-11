using System;
using System.Runtime.Serialization;

namespace ProjectManager.Common.Encryption;

/// <summary>
/// Represents an exception that occurs during encryption operations.
/// </summary>
[Serializable]
public class EncryptionException : Exception
{
    /// <summary>
    /// Represents an exception that is thrown when an encryption operation fails.
    /// </summary>
    /// <remarks>
    /// This exception is thrown when there is an error during the encryption process.
    /// </remarks>
    public EncryptionException(string message) : base(message)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when an encryption operation fails.
    /// </summary>
    /// <remarks>
    /// This exception is thrown when there is an error during the encryption process.
    /// </remarks>
    public EncryptionException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when an encryption operation fails.
    /// </summary>
    /// <remarks>
    /// This exception is thrown when there is an error during the encryption process.
    /// </remarks>
    protected EncryptionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}