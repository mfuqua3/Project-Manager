using System.Runtime.Serialization;

namespace ProjectManager.Tests.Infrastructure;
[Serializable]
public class TestResourceAccessException :Exception
{
    public TestResourceAccessException(string message):base(message)
    {
        
    }

    protected TestResourceAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        
    }
}