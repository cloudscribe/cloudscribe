using System;
namespace cloudscribe.Core.Models.Geography
{
    public interface ILanguage
    {
        string Code { get; set; }
        Guid Guid { get; set; }
        string Name { get; set; }
        int Sort { get; set; }
    }
}
