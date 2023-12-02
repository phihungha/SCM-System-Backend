namespace ScmssApiServer.Models
{
    public abstract class TransOrderEvent : OrderEvent
    {
        public TransOrderEventType Type { get; set; }
        public override bool IsAutomatic => Type != TransOrderEventType.Left
                                            && Type != TransOrderEventType.Arrived
                                            && Type != TransOrderEventType.Interrupted;
    }
}
