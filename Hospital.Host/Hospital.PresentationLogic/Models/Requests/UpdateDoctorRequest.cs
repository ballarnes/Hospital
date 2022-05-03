using System.ComponentModel.DataAnnotations;

namespace Hospital.PresentationLogic.Models.Requests
{
    public class UpdateDoctorRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; } = null!;

        [StringLength(50)]
        public string Surname { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int SpecializationId { get; set; }
    }
}
