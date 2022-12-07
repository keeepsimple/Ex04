using Ex04.Models.BaseEntities;

namespace Ex04.Models
{
    public class ImageCategory : BaseEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<@int> ImageAndCategories { get; set; }
    }
}
