using Ex04.Models.BaseEntities;

namespace Ex04.Models
{
    public class @int:BaseEntity
    {
        public int ImageCategoryId { get; set; }

        public virtual ImageCategory ImageCategory { get; set; }

        public int ImageId { get; set; }

        public virtual Image Image { get; set; }
    }
}
