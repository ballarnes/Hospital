namespace Hospital.Host.Models.Requests
{
    public class PaginatedItemsRequest
    {
        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; }

        [Range(0, int.MaxValue)]
        public int PageSize { get; set; }
    }
}
