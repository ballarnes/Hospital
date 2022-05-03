using System.ComponentModel.DataAnnotations;

namespace Hospital.PresentationLogic.Models.Requests
{
    public class GetByIdRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
