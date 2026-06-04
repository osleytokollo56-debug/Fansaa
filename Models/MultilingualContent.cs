using System.ComponentModel.DataAnnotations;

namespace SomaShare.Models
{
    public class MultilingualContent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; } // e.g., "btn_submit", "msg_welcome"

        [Required]
        [StringLength(50)]
        public string Language { get; set; } // e.g., "en", "xh", "zu", "af"

        [Required]
        [StringLength(1000)]
        public string Value { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }

    public class LanguagePreference
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string PreferredLanguage { get; set; } = "en";

        public DateTime SetDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public ApplicationUser User { get; set; }
    }
}
