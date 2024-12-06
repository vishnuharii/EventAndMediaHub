namespace EventAndMediaHub.Models
{
    public class ServiceResponse
    {
        public enum ServiceStatus
        {

            NotFound,   // The requested resource was not found
            Created,    // A new resource was created successfully
            Updated,    // An existing resource was updated successfully
            Deleted,    // A resource was deleted successfully
            Error,      // An error occurred during the operation
            Conflict,   // A conflict occurred (e.g., data integrity issue)
            Success      // Operation completed successfully without any specific issues
        }

        /// <summary>
        /// Gets or sets the status of the service response.
        /// </summary>
        public ServiceStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the ID of the created resource, if applicable.
        /// </summary>
        public int CreatedId { get; set; }

        /// <summary>
        /// Gets or sets the list of messages related to the service operation.
        /// This can include validation or logic errors.
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();
    }
}

