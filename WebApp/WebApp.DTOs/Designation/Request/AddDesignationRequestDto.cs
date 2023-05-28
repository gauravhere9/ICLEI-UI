using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;

namespace WebApp.DTOs.Designation.Request
{
    public class AddDesignationRequestDto
    {
        [Required(ErrorMessage = "Designation name is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string Description { get; set; } = string.Empty;
    }
}
