using System.Collections.Generic;

namespace Hospital.PresentationLogic.Models.Responses
{
    public class ArrayResponse<T>
    {
        public int TotalCount { get; init; }

        public IEnumerable<T> Data { get; init; } = null!;
    }
}