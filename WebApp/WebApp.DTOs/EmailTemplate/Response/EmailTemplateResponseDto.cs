namespace WebApp.DTOs.EmailTemplate.Response
{
    public class EmailTemplateResponseDto
    {
        public int Id { get; set; }
        public string TemplateName { get; set; } = string.Empty;
        public string TemplateBody { get; set; } = string.Empty;
        public string DefaultBody { get; set; } = string.Empty;
        public string SubjectLine { get; set; } = string.Empty;
        public string TemplateTags { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? DeactivatedDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
