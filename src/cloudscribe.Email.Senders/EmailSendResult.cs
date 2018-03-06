namespace cloudscribe.Email
{
    public class EmailSendResult
    {
        public EmailSendResult(bool succeeded, string errorMessage = null)
        {
            Succeeded = succeeded;
            ErrorMessage = errorMessage;

        }

        public bool Succeeded { get; private set; }
        public string ErrorMessage { get; private set; }
    }
}
