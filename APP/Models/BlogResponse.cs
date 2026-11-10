using CORE.Domain;
using System.ComponentModel;

namespace APP.Models
{
    // Response properties are created according to the data shown in views.
    public class BlogResponse : Record
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public decimal? Rating { get; set; }
        public DateTime PublishDate { get; set; }
        public bool IsArchived { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("User ID")]
        public int? UserId { get; set; }

        // Custom and formatted response properties end with R.
        [DisplayName("Rating")]
        public string RatingR { get; set; }

        [DisplayName("Publish Date")]
        public string PublishDateR { get; set; }

        [DisplayName("Archived")]
        public string IsArchivedR { get; set; }

        [DisplayName("Tags")]
        public string TagNamesR { get; set; }
    }
}