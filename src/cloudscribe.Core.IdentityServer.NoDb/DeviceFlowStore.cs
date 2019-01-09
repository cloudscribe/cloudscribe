using cloudscribe.Core.IdentityServer.NoDb.Models;
using cloudscribe.Core.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.NoDb
{
    public class DeviceFlowStore : IDeviceFlowStore
    {
        public DeviceFlowStore(
            IHttpContextAccessor contextAccessor,
            IBasicQueries<DeviceFlowCodes> queries,
            IBasicCommands<DeviceFlowCodes> commands,
            IPersistentGrantSerializer serializer,
            ILogger<DeviceFlowStore> logger
            )
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _queries = queries;
            _commands = commands;
            _serializer = serializer;
        }

        private IHttpContextAccessor _contextAccessor;
        private readonly ILogger _logger;
        private readonly IPersistentGrantSerializer _serializer;
        private IBasicQueries<DeviceFlowCodes> _queries;
        private IBasicCommands<DeviceFlowCodes> _commands;

        private DeviceFlowCodes ToEntity(DeviceCode model, string deviceCode, string userCode, string siteId)
        {
            if (model == null || deviceCode == null || userCode == null) return null;

            return new DeviceFlowCodes
            {
                SiteId = siteId,
                DeviceCode = deviceCode,
                UserCode = userCode,
                ClientId = model.ClientId,
                SubjectId = model.Subject?.FindFirst(JwtClaimTypes.Subject).Value,
                CreationTime = model.CreationTime,
                Expiration = model.CreationTime.AddSeconds(model.Lifetime),
                Data = _serializer.Serialize(model)
            };
        }

        private DeviceCode ToModel(string entity)
        {
            if (entity == null) return null;

            return _serializer.Deserialize<DeviceCode>(entity);
        }

        private async Task<IEnumerable<DeviceFlowCodes>> GetAllInternalAsync(string siteId)
        {
            //TODO: cache
            var all = await _queries.GetAllAsync(siteId).ConfigureAwait(false);
            return all;
        }

        public async Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return null;
            }
            var siteId = site.Id.ToString();

            var all = await GetAllInternalAsync(siteId);

            var deviceFlowCodes = all.FirstOrDefault(x => x.DeviceCode == deviceCode && x.SiteId == siteId);
            
            var model = ToModel(deviceFlowCodes?.Data);
            return model;

        }

        public async Task<DeviceCode> FindByUserCodeAsync(string userCode)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return null;
            }
            var siteId = site.Id.ToString();

            var all = await GetAllInternalAsync(siteId);

            var deviceFlowCodes = all
                .FirstOrDefault(x => x.UserCode == userCode && x.SiteId == siteId);

            var model = ToModel(deviceFlowCodes?.Data);
            return model;
        }

        public async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return;
            }
            var siteId = site.Id.ToString();

            var entity = ToEntity(data, deviceCode, userCode, siteId);

            await _commands.CreateAsync(siteId, userCode, entity);

        }

        public async Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return;
            }
            var siteId = site.Id.ToString();

            var all = await GetAllInternalAsync(siteId);

            var existing = all
                .FirstOrDefault(x => x.UserCode == userCode && x.SiteId == siteId);

            if (existing == null)
            {
                _logger.LogError("{userCode} not found in database", userCode);
                throw new InvalidOperationException("Could not update device code");
            }

            var entity = ToEntity(data, existing.DeviceCode, userCode, siteId);
            _logger.LogDebug("{userCode} found in database", userCode);

            existing.SubjectId = data.Subject?.FindFirst(JwtClaimTypes.Subject).Value;
            existing.Data = entity.Data;

            await _commands.UpdateAsync(siteId, existing.UserCode, existing);
        }

        public async Task RemoveByDeviceCodeAsync(string deviceCode)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return;
            }
            var siteId = site.Id.ToString();


            var all = await GetAllInternalAsync(siteId);

            var deviceFlowCodes = all.FirstOrDefault(x => x.DeviceCode == deviceCode && x.SiteId == siteId);
            
            if (deviceFlowCodes != null)
            {
                _logger.LogDebug("removing {deviceCode} device code from database", deviceCode);

                await _commands.DeleteAsync(siteId, deviceFlowCodes.UserCode);
            }
            else
            {
                _logger.LogDebug("no {deviceCode} device code found in database", deviceCode);
            }
        }


    }
}
