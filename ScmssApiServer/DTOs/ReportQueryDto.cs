using System.ComponentModel.DataAnnotations;

namespace ScmssApiServer.DTOs
{
    public class ReportQueryDto
    {
        [Range(1, 12)]
        public int EndMonth { get; set; }

        [Range(1970, int.MaxValue)]
        public int EndYear { get; set; }

        [Range(1, 12)]
        public int StartMonth { get; set; }

        [Range(1970, int.MaxValue)]
        public int StartYear { get; set; }
    }
}
