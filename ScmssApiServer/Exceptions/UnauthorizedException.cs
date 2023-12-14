namespace ScmssApiServer.Exceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException(string? message = "Forbidden.")
            : base(message)
        {
        }
    }
}
