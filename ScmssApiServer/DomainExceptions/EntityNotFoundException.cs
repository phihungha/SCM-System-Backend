namespace ScmssApiServer.DomainExceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException(
                string? message = "Entity not found"
            )
            : base(message)
        {
        }
    }
}
