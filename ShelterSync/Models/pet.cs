using System.ComponentModel.DataAnnotations;

namespace ShelterHelper.Models
{
    /// <summary>
    /// Represents a pet in the shelter system.
    /// Includes properties for pet details, adoption status, and media.
    /// </summary>
    public class Pet
    {
        /// <summary>
        /// Unique identifier for the pet.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the pet. Required field.
        /// </summary>
        [Required(ErrorMessage = "Pet name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Pet name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Species of the pet (e.g., Dog, Cat, Rabbit).
        /// </summary>
        [Required(ErrorMessage = "Species is required")]
        public string Species { get; set; } = "Dog";

        /// <summary>
        /// Breed of the pet. Optional field.
        /// </summary>
        [StringLength(100)]
        public string? Breed { get; set; }

        /// <summary>
        /// Age of the pet in years.
        /// </summary>
        [Range(0, 50, ErrorMessage = "Age must be between 0 and 50")]
        public int Age { get; set; }

        /// <summary>
        /// Additional notes about the pet (temperament, health, etc.).
        /// </summary>
        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        /// <summary>
        /// File path to the pet's image. Stored as URL path.
        /// </summary>
        public string? ImagePath { get; set; }

        /// <summary>
        /// Indicates whether the pet has been adopted.
        /// </summary>
        public bool IsAdopted { get; set; } = false;

        /// <summary>
        /// Indicates whether the pet is featured as Pet of the Week.
        /// </summary>
        public bool IsPetOfTheWeek { get; set; } = false;
    }
}