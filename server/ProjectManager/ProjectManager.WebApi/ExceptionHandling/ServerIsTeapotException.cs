using System.Runtime.Serialization;

namespace ProjectManager.WebApi.ExceptionHandling;

[Serializable]
public class ServerIsTeapotException : Exception
{
    public ServerIsTeapotException():base("I'm a teapot")
    {   
    }
    
    protected ServerIsTeapotException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public static void Throw() => throw new ServerIsTeapotException();
}