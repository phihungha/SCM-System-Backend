namespace ScmssApiServer.Models
{
    public enum ProductionOrderEventType
    {
        PendingApproval,
        Approved,
        Rejected,
        Started,
        StageDone,
        Produced,
        Completed,
        Canceled,
        Unaccepted,
        Interrupted,
    }
}
