namespace cloudscribe.Core.IdentityServer.EFCore.Entities
{
    public class ApiResourceProperty
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public int ApiResourceId { get; set; }
        public ApiResource ApiResource { get; set; }
    }
}
