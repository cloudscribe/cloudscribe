// Author:					Joe Audette
// Created:					2015-01-10
// Last Modified:			2015-06-23
// 
using System;

namespace cloudscribe.Configuration
{
    public interface IVersionProvider
    {
        string Name { get; }
        Guid ApplicationId { get; }
        Version GetCodeVersion();
    }
}
