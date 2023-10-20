namespace ScmssApiServer.Exceptions
{
    public class AppConfigException : ApplicationException
    {
        public AppConfigException(string message)
            : base(message)
        {
        }
    }
}
