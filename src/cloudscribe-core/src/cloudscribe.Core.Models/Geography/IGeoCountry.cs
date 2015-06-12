using System;
namespace cloudscribe.Core.Models.Geography
{
    public interface IGeoCountry
    {
        Guid Guid { get; set; }
        string ISOCode2 { get; set; }
        string ISOCode3 { get; set; }
        string Name { get; set; }
    }
}
