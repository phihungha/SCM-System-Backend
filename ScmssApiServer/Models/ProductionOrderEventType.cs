namespace ScmssApiServer.Models
{
    public enum ProductionOrderEventType
    {
        PendingApproval,
        Approved,
        Rejected,
        Producing,
        StageDone,
        Produced,
        Completed,
        Canceled,
        Unaccepted,
        Interrupted,
    }
}
