using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;

namespace WebApp.DTOs.Branches.Request
{
    public class UpdateBranchRequestDto
    {
        [Required(ErrorMessage = "Branch id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Branch code is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string BranchCode { get; set; }

        [Required(ErrorMessage = "Branch address is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string Address { get; set; }

        [Required(ErrorMessage = "Branch location is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string Location { get; set; }

        [Required(ErrorMessage = "Company id is required")]
        public int CompanyId { get; set; }
    }
}
