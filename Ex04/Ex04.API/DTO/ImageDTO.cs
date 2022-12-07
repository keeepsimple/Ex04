namespace Ex04.API.DTO
{
    public class ImageDTO
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public decimal Size { get; set; }

        public decimal Height { get; set; }

        public decimal Width { get; set; }

        public int? PostId { get; set; }

        public IFormFile UploadImage { get; set; }

        public IList<int> ImageCateIds { get; set; }
    }
}
