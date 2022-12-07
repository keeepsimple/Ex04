using Ex04.Models.BaseEntities;

namespace Ex04.Models
{
    public class Category : BaseEntity
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}