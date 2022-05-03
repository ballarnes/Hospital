using System.ComponentModel.DataAnnotations;

namespace Hospital.PresentationLogic.Models.Requests
{
    public class UpdateSpecializationRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; } = null!;

        [StringLength(300)]
        public string Description { get; set; } = null!;
    }
}
