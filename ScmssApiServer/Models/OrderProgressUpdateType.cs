namespace ScmssApiServer.Models
{
    public enum OrderProgressUpdateType
    {
        Processing,
        Left,
        Arrived,
        Delivered,
        PaymentDue,
        PaymentCompleted,
        Canceled,
        Returned,
        Interrupted,
    }
}
