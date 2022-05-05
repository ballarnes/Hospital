using System.Collections.Generic;

namespace Hospital.BusinessLogic.Models.Responses
{
    public class ArrayResponse<T>
    {
        public int TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}