using System;
using System.Collections.Generic;

namespace cloudscribe.Caching
{
	public class DistributedCacheFactoryBase
	{
		public List<ServerNode> ParseConfig(string configValue)
		{
			List<ServerNode> config = new List<ServerNode>();
#if NET35
            if (String.IsNullOrEmpty(configValue))
                return config;
#else
			if (String.IsNullOrWhiteSpace(configValue))
				return config;
#endif

			var endPointList = configValue.Split(',');
			if (endPointList.Length == 0)
				return config;

			foreach (var endpoint in endPointList)
			{
				var endPointComponents = endpoint.Split(':');
				if (endPointComponents.Length < 2)
					continue;

				int port;
				if (int.TryParse(endPointComponents[1], out port))
				{
					var cacheEndpoint = new ServerNode(endPointComponents[0], port);
					config.Add(cacheEndpoint);
				}
			}

			return config;
		}

	}
}
