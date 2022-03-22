namespace Hospital.Host.Models.Responses
{
    public class IdResponse<T>
    {
        public T Id { get; set; } = default(T)!;
    }
}
