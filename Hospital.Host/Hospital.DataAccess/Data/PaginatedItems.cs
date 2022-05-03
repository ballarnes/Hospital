using System.Collections.Generic;

namespace Hospital.DataAccess.Data
{
    public class PaginatedItems<T>
    {
        public int PagesCount { get; init; }

        public int TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}
