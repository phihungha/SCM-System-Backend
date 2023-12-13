namespace ScmssApiServer.DomainExceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException(string? message = "Forbidden.")
            : base(message)
        {
        }
    }
}
