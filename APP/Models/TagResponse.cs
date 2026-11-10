using CORE.Domain;
using System.ComponentModel;

namespace APP.Models
{
    public class TagResponse : Record
    {
        public string Name { get; set; }

        [DisplayName("Blogs")]
        public string BlogTitlesR { get; set; }

        [DisplayName("Blog Count")]
        public int BlogCountR { get; set; }
    }
}