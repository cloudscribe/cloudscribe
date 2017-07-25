namespace cloudscribe.FileManager.Web.Models
{
    public class OperationResult
    {
        public OperationResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public bool Succeeded { get; private set; }

        public string Message { get; set; }
    }
}
