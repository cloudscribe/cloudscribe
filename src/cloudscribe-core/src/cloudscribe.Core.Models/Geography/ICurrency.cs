using System;
namespace cloudscribe.Core.Models.Geography
{
    public interface ICurrency
    {
        string Code { get; set; }
        DateTime Created { get; set; }
        string DecimalPlaces { get; set; }
        string DecimalPointChar { get; set; }
        Guid Guid { get; set; }
        DateTime LastModified { get; set; }
        string SymbolLeft { get; set; }
        string SymbolRight { get; set; }
        string ThousandsPointChar { get; set; }
        string Title { get; set; }
        decimal Value { get; set; }
    }
}
