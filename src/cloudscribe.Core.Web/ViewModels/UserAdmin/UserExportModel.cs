namespace cloudscribe.Core.Web.ViewModels.UserAdmin
{
    public class UserExportModel
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CreatedUtc { get; set; } = string.Empty;
    }
}