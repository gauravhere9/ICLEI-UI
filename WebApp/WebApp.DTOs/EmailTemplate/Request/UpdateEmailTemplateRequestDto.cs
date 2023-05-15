using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;

namespace WebApp.DTOs.EmailTemplate.Request
{
    public class UpdateEmailTemplateRequestDto
    {
        [Required(ErrorMessage = "Email template id is required.")]
        [NoConsecutiveSpace]
        [NoSpaceChar]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter valid email template id.")]
        [DefaultValue(0)]
        public int Id { get; set; }

        [NoConsecutiveSpace]
        [Required(ErrorMessage = "Email template name is required.")]
        public string TemplateName { get; set; } = string.Empty;

        [NoConsecutiveSpace]
        [Required(ErrorMessage = "Email template body is required.")]
        public string TemplateBody { get; set; } = string.Empty;

        [NoConsecutiveSpace]
        [Required(ErrorMessage = "Subject line  is required.")]
        public string SubjectLine { get; set; } = string.Empty;

        public string TemplateTags { get; set; } = string.Empty;
    }
}
