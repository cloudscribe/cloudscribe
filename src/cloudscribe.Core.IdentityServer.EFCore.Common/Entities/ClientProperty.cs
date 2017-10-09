#pragma warning disable 1591

namespace cloudscribe.Core.IdentityServer.EFCore.Entities
{
    public class ClientProperty
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public Client Client { get; set; }
    }
}
