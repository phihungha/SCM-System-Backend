namespace ScmssApiServer.Models
{
    public abstract class TransOrderEvent : OrderEvent
    {
        public TransOrderEventType Type { get; set; }
    }
}
