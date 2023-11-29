namespace ScmssApiServer.Models
{
    public enum TransOrderEventType
    {
        Processing,
        DeliveryStarted,
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
