namespace IndieForge.DTOs.Dashboard
{
    public class LogsAuditoryDto
    {
        public string Message { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public Guid TargetId { get; set; }
        public DateTime DateOcurrency { get; set; }
    }
}
