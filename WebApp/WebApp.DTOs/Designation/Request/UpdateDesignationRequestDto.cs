namespace WebApp.DTOs.Designation.Request
{
    public class UpdateDesignationRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
