using CORE.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    // Tag is a Record.
    public class Tag : Record
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        // For tags-blogs many to many relationship.
        public List<BlogTag> BlogTags { get; set; } = new();
    }
}