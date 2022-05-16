using System.Collections.Generic;

namespace Hospital.PresentationLogic.Models.Responses
{
    public class PaginatedItemsResponse<T>
    {
        public int PageIndex { get; init; }

        public int PageSize { get; init; }

        public int PagesCount { get; init; }

        public int TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}
