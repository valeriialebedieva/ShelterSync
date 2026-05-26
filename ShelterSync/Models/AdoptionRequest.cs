using System.ComponentModel.DataAnnotations;

namespace ShelterHelper.Models
{
    public class AdoptionRequest
    {
        public int Id { get; set; }

        [Required]
        public int PetId { get; set; }

        [Required]
        [StringLength(100)]
        public string RequesterName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        [StringLength(1000)]
        public string? Message { get; set; }

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";
    }
}