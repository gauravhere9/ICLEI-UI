using System.ComponentModel.DataAnnotations;
using WebApp.Global.Attributes;

namespace WebApp.DTOs.Holiday.Request
{
    public class UpdateHolidayRequestDto
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [NoSpaceChar]
        [NoConsecutiveSpace]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Holiday date is required")]
        public DateTime HolidayDate { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Holiday type is required")]
        public int HolidayTypeId { get; set; }
    }
}
