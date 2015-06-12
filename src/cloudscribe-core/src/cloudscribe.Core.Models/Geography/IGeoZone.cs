using System;
namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoZone
    {
        string Code { get; set; }
        Guid CountryGuid { get; set; }
        Guid Guid { get; set; }
        string Name { get; set; }
    }
}
