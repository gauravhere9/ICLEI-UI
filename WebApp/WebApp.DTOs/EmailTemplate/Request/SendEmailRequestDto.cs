using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;
using static WebApp.Global.Constants.Enumurations;

namespace WebApp.DTOs.EmailTemplate.Request
{
    public class SendEmailRequestDto
    {
        public SendEmailRequestDto()
        {
            this.Receivers = new List<string>();
            this.Tags = new Dictionary<string, string>();
            this.Attachments = new Dictionary<string, MemoryStream>();
        }

        [Required(ErrorMessage = "Atleast one receiver email is required.")]
        [NotEmptyStringList]
        public List<string>? Receivers { get; set; }

        [Required(ErrorMessage = "Email Template is required.")]
        public EmailTemplateTypes EmailTemplate { get; set; }

        [Required(ErrorMessage = "Email tags are required.")]
        public Dictionary<string, string>? Tags { get; set; }

        public Dictionary<string, MemoryStream>? Attachments { get; set; }
    }
}
