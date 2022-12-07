using Ex04.Models.BaseEntities;

namespace Ex04.Models
{
    public class Comment : BaseEntity 
    {
        public string UserId { get; set; }

        public string CommentContent { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }
    }
}