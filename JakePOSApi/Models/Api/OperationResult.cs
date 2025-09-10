namespace JakePOSApi.Models.Api
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

        public static OperationResult SuccessResult(string message = "") => new() { Success = true, Message = message };
        public static OperationResult FailureResult(List<string> errors) => new() { Success = false, Errors = errors };
    }
}
