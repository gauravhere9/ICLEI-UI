using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;

namespace WebApp.DTOs.FundingAgency.Request
{
    public class UpdateFundingAgencyRequestDto
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contac person is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string ContactPerson { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
