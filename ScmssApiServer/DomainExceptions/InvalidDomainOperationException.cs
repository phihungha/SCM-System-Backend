namespace ScmssApiServer.DomainExceptions
{
    public class InvalidDomainOperationException : ApplicationException
    {
        public InvalidDomainOperationException(
                string? message = "Domain operation is invalid."
            )
            : base(message)
        {
        }
    }
}
