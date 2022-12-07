namespace Ex04.API.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public string Content { get; set; }

        public int RateCount { get; set; }

        public decimal Rate { get; set; }

        public int Views { get; set; }

        public int CategoryId { get; set; }
    }
}
