using CORE.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Domain
{
    
    public class Blog : Record
    {
        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public decimal? Rating { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsArchived { get; set; }

        [Required, StringLength(30)]
        public string UserName { get; set; }

        public int? UserId { get; set; }

        
        public List<BlogTag> BlogTags { get; set; } = new();

       
        [NotMapped]
        public List<int> TagIds
        {
            get => BlogTags.Select(blogTagEntity
                => blogTagEntity.TagId).ToList();

            set => BlogTags = value.Select(tagIdValue => new BlogTag
            {
                TagId = tagIdValue
            }).ToList();
        }
    }
}