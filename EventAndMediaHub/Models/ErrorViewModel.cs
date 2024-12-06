namespace EventAndMediaHub.Models
{
    public class ErrorViewModel
    {
        internal List<string> Errors;
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
