namespace ScmssApiServer.DomainExceptions
{
    public class AuthException : ApplicationException
    {
        public AuthException(
                string? message = "Authentication failed."
            )
            : base(message)
        {
        }
    }
}
