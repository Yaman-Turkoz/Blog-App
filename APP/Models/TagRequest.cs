using CORE.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class TagRequest : Record
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }
    }
}