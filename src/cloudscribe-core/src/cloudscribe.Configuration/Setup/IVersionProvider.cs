// Author:					Joe Audette
// Created:					2015-01-10
// Last Modified:			2015-06-12
// 
using System;

namespace cloudscribe.Configuration
{
    public interface IVersionProvider
    {
        string Name { get; }
        Version GetCodeVersion();
    }
}
