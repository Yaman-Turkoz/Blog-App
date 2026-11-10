using CORE.Domain;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    // Request properties are created according to the data from views.
    public class BlogRequest : Record
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        public string Content { get; set; }

        public decimal? Rating { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [DisplayName("Publish Date")]
        public DateTime? PublishDate { get; set; }

        [DisplayName("Archived")]
        public bool IsArchived { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(30, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("User ID")]
        public int? UserId { get; set; }

        [DisplayName("Tags")]
        public List<int> TagIds { get; set; } = new();
    }
}