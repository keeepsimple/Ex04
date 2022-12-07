using Ex04.Models.BaseEntities;

namespace Ex04.Models
{
    public class Rate : BaseEntity
    {
        public int TotalRate { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
