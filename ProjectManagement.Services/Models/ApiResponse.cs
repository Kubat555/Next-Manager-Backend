

namespace ProjectManagement.Services.Models
{
    public class ApiResponse<T>
    {
        public bool isSuccess {get;set;}
        public string? Message { get;set;}
        public int StatusCode { get;set;}
        public T? Response { get;set;}
    }
}
