namespace Ex04.API.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int PostId { get; set; }

        public string CommentContent { get; set; }
    }
}
