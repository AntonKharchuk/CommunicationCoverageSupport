namespace CommunicationCoverageSupport.PresentationBlazor.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }
    }
}
