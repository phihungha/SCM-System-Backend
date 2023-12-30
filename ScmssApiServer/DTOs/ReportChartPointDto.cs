namespace ScmssApiServer.DTOs
{
    public class ReportChartPointDto<TName, TValue>
    {
        public required TName Name { get; set; }
        public required TValue Value { get; set; }
    }
}
