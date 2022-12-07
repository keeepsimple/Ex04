using Ex04.Models.BaseEntities;

namespace Ex04.Models
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public string Content { get; set; }

        public int RateCount { get; set; }

        public decimal Rate { get; set; }

        public int Views { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}