namespace ScmssApiServer.Models
{
    public enum OrderStatus
    {
        Processing,
        Executing,
        Interrupted,
        WaitingAcceptance,
        Completed,
        Canceled,
        Returned,
    }
}
