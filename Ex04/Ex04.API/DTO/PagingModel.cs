namespace Ex04.API.DTO
{
    public class PagingModel<T>
    {
        public IEnumerable<T> List { get; set; }

        public int Count { get; set; }
    }
}
