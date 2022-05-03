namespace Hospital.BusinessLogic.Models.Responses
{
    public class IdResponse<T>
    {
        public T Id { get; set; } = default(T)!;
    }
}
