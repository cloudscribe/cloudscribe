//  Author:                     Joe Audette
//  Created:                    2014-10-09
//	Last Modified:              2014-10-09
// 

using log4net;
using System;
using System.Configuration.Provider;
using System.Web.Configuration;

namespace cloudscribe.Configuration
{
    public sealed class VersionProviderManager
    {
        private static object initializationLock = new object();
        private static readonly ILog log
            = LogManager.GetLogger(typeof(VersionProviderManager));

        static VersionProviderManager()
        {
            Initialize();
        }

        private static void Initialize()
        {
            providerCollection = new VersionProviderCollection();

            try
            {
                VersionProviderConfig config
                    = VersionProviderConfig.GetConfig();

                if (config != null)
                {

                    if (
                        (config.Providers == null)
                        || (config.Providers.Count < 1)
                        )
                    {
                        throw new ProviderException("No VersionProviderCollection found.");
                    }

                    ProvidersHelper.InstantiateProviders(
                        config.Providers,
                        providerCollection,
                        typeof(VersionProvider));

                }
                else
                {
                    // config was null, not a good thing
                    log.Error("VersionProviderConfig could not be loaded so empty provider collection was returned");

                }
            }
            catch (NullReferenceException ex)
            {
                log.Error(ex);
            }
            catch (TypeInitializationException ex)
            {
                log.Error(ex);
            }
            catch (ProviderException ex)
            {
                log.Error(ex);
            }
            
            providerCollection.SetReadOnly();
                
            
        }


        private static VersionProviderCollection providerCollection;

        public static VersionProviderCollection Providers
        {
            get
            {
                if (providerCollection == null) Initialize();
                return providerCollection;
                
            }
        }
    }
}
