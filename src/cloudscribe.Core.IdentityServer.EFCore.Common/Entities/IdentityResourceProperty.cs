namespace cloudscribe.Core.IdentityServer.EFCore.Entities
{
    public class IdentityResourceProperty
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public int IdentityResourceId { get; set; }
        public IdentityResource IdentityResource { get; set; }
    }
}
