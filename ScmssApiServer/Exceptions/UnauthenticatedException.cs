namespace ScmssApiServer.Exceptions
{
    public class UnauthenticatedException : ApplicationException
    {
        public UnauthenticatedException(string? message = "Authentication failed.")
            : base(message)
        {
        }
    }
}
