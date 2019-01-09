using cloudscribe.Core.IdentityServer.EFCore.Entities;
using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
using cloudscribe.Core.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Stores.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.IdentityServer.EFCore.Stores
{
    public class DeviceFlowStore : IDeviceFlowStore
    {
        public DeviceFlowStore(
            IHttpContextAccessor contextAccessor,
            IPersistedGrantDbContext context,
            IPersistentGrantSerializer serializer,
            ILogger<DeviceFlowStore> logger
            )
        {
            _contextAccessor = contextAccessor;
            _context = context;
            _serializer = serializer;
            _logger = logger;
        }

        private readonly IPersistedGrantDbContext _context;
        private readonly IPersistentGrantSerializer _serializer;
        private readonly ILogger _logger;
        private IHttpContextAccessor _contextAccessor;

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

        public async Task<DeviceCode> FindByDeviceCodeAsync(string deviceCode)
        {
            var site = _contextAccessor.HttpContext.GetTenant<SiteContext>();
            if (site == null)
            {
                _logger.LogError("sitecontext was null");
                return null;
            }
            var siteId = site.Id.ToString();

            var deviceFlowCodes = await _context.DeviceFlowCodes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.DeviceCode == deviceCode && x.SiteId == siteId);

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

            var deviceFlowCodes = await _context.DeviceFlowCodes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserCode == userCode && x.SiteId == siteId);

            var model = ToModel(deviceFlowCodes?.Data);
            return model;
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

            var deviceFlowCodes = await _context.DeviceFlowCodes
                .FirstOrDefaultAsync(x => x.DeviceCode == deviceCode && x.SiteId == siteId);

            if (deviceFlowCodes != null)
            {
                _logger.LogDebug("removing {deviceCode} device code from database", deviceCode);

                _context.DeviceFlowCodes.Remove(deviceFlowCodes);

                try
                {
                   await  _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogInformation("exception removing {deviceCode} device code from database: {error}", deviceCode, ex.Message);
                }
            }
            else
            {
                _logger.LogDebug("no {deviceCode} device code found in database", deviceCode);
            }
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

            _context.DeviceFlowCodes.Add(ToEntity(data, deviceCode, userCode, siteId));
            
            await _context.SaveChangesAsync().ConfigureAwait(false);
            
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

            var existing = await _context.DeviceFlowCodes
                .SingleOrDefaultAsync(x => x.UserCode == userCode && x.SiteId == siteId);

            if (existing == null)
            {
                _logger.LogError("{userCode} not found in database", userCode);
                throw new InvalidOperationException("Could not update device code");
            }

            var entity = ToEntity(data, existing.DeviceCode, userCode, siteId);
            _logger.LogDebug("{userCode} found in database", userCode);

            existing.SubjectId = data.Subject?.FindFirst(JwtClaimTypes.Subject).Value;
            existing.Data = entity.Data;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning("exception updating {userCode} user code in database: {error}", userCode, ex.Message);
            }
        }
    }
}
