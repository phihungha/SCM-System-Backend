namespace ScmssApiServer.Models
{
    public enum ProductionOrderStatus
    {
        PendingApproval,
        Approved,
        Rejected,
        Started,
        Produced,
        Completed,
        Canceled,
        Unaccepted,
    }
}
