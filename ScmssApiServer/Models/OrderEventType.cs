namespace ScmssApiServer.Models
{
    public enum OrderEventType
    {
        Processing,
        Left,
        Arrived,
        Delivered,
        Completed,
        PaymentDue,
        PaymentCompleted,
        Canceled,
        Returned,
        Interrupted,
    }
}
