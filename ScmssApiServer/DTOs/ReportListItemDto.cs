namespace ScmssApiServer.DTOs
{
    public class ReportListItemDto<TItem, TValue>
    {
        public required TItem Item { get; set; }
        public required TValue Value { get; set; }
    }
}
