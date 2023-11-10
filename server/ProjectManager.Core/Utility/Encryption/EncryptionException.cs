using System.Runtime.Serialization;

namespace ProjectManager.Core.Utility.Encryption;

[Serializable]
public class EncryptionException : Exception
{
    public EncryptionException(string message) : base(message)
    {
    }

    public EncryptionException(string message, Exception inner) : base(message, inner)
    {
    }

    protected EncryptionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}