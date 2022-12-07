using Ex04.Models.BaseEntities;

namespace Ex04.Models
{
    public class Image : BaseEntity
    {
        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public decimal Size { get; set; }

        public decimal Height { get; set; }

        public decimal Width { get; set; }

        public int? PostId { get; set; }

        public virtual Post Post { get; set; }

        public virtual ICollection<@int> ImageAndCategories { get; set; }
    }
}
