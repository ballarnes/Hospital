using System.ComponentModel.DataAnnotations;

namespace Hospital.PresentationLogic.Models.Requests
{
    public class AddSpecializationRequest
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [StringLength(300)]
        public string Description { get; set; } = null!;
    }
}
