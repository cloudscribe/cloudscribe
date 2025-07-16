using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using Microsoft.Extensions.Logging;
using NoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace cloudscribe.Core.Storage.NoDb
{
    public class IpAddressCommands : IipAddressCommands
    {
        private IBasicCommands<BlockedPermittedIpAddressesModel> _blockedPermittedIpAddressesCommands;
        private IBasicQueries<BlockedPermittedIpAddressesModel> _blockedPermittedIpAddressesQueries;
        private ILogger _log;

        public IpAddressCommands(IBasicCommands<BlockedPermittedIpAddressesModel> blockedPermittedIpAddressesCommands, IBasicQueries<BlockedPermittedIpAddressesModel> blockedPermittedIpAddressesQueries, ILogger<IpAddressCommands> logger)
        {
            _blockedPermittedIpAddressesCommands = blockedPermittedIpAddressesCommands;
            _blockedPermittedIpAddressesQueries = blockedPermittedIpAddressesQueries;
            _log = logger;
        }

        public async Task<PagedResult<BlockedPermittedIpAddressesModel>> GetPermittedIpAddresses(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken), bool? IsFromService = false)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0) throw new ArgumentNullException("must include site ID");

            int offset = (pageSize * pageNumber) - pageSize;
            IEnumerable<BlockedPermittedIpAddressesModel> data;
            IEnumerable<BlockedPermittedIpAddressesModel> results = await _blockedPermittedIpAddressesQueries.GetAllAsync(siteId.ToString(), cancellationToken).ConfigureAwait(false);

            if ((bool)IsFromService)
            {
                data = results.OrderBy(x => x.CreatedDate)
                            .Where(x => x.SiteId == siteId && x.IsPermitted == true)
                            .ToList().Skip(offset);
            }
            else
            {
                data = results.OrderBy(x => x.CreatedDate)
                        .Where(x => x.SiteId == siteId && x.IsPermitted == true)
                        .ToList()
                        .Skip(offset)
                        .Take(pageSize);
            }

            PagedResult<BlockedPermittedIpAddressesModel> result = new PagedResult<BlockedPermittedIpAddressesModel>();

            result.Data = data.Where(x => x.SiteId == siteId && x.IsPermitted == true).ToList();
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;

            result.TotalItems = results.Where(s => s.SiteId == siteId && s.IsPermitted == true).Count();

            return result;
        }

        public async Task<bool> AddPermittedIpAddress(BlockedPermittedIpAddressesModel blockedPermittedIpAddressesModel, Guid currentSiteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (blockedPermittedIpAddressesModel == null)
            {
                _log.LogError($"IP Address Model is required");
                throw new ArgumentException($"IP Address Model is required");
            }
            if (blockedPermittedIpAddressesModel.IpAddress.Length <= 0)
            {
                _log.LogError($"IP Address is required");
                throw new ArgumentException($"IP Address is required");
            }
            if (blockedPermittedIpAddressesModel.SiteId.ToString() == null)
            {
                _log.LogError($"Site ID is required");
                throw new ArgumentException($"Site ID is required");
            }

            try
            {
                await _blockedPermittedIpAddressesCommands.CreateAsync(currentSiteId.ToString(), blockedPermittedIpAddressesModel.Id.ToString(), blockedPermittedIpAddressesModel).ConfigureAwait(false);

                return true;
            }
            catch (Exception e)
            {
                _log.LogError($"Error creating permitted IP Address");
                return false;
            }
        }

        public async Task<bool> UpdatePermittedIpAddress(BlockedPermittedIpAddressesModel blockedPermittedIpAddressesModel, Guid currentSiteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (blockedPermittedIpAddressesModel == null)
            {
                _log.LogError($"IP Address Model is required");
                throw new ArgumentException($"IP Address Model is required");
            }
            if (blockedPermittedIpAddressesModel.IpAddress.Length <= 0)
            {
                _log.LogError($"IP Address is required");
                throw new ArgumentException($"IP Address is required");
            }
            if (blockedPermittedIpAddressesModel.SiteId.ToString() == null)
            {
                _log.LogError($"Site ID is required");
                throw new ArgumentException($"Site ID is required");
            }

            try
            {
                IEnumerable<BlockedPermittedIpAddressesModel> results = await _blockedPermittedIpAddressesQueries.GetAllAsync(currentSiteId.ToString(), cancellationToken).ConfigureAwait(false);

                var entity = results.FirstOrDefault(x => x.Id == blockedPermittedIpAddressesModel.Id && x.SiteId == blockedPermittedIpAddressesModel.SiteId && blockedPermittedIpAddressesModel.IsPermitted == true);

                if (entity == null)
                    return false;

                entity.IpAddress = blockedPermittedIpAddressesModel.IpAddress;
                entity.Reason = blockedPermittedIpAddressesModel.Reason;
                entity.LastUpdated = blockedPermittedIpAddressesModel.LastUpdated;
                entity.IsRange = blockedPermittedIpAddressesModel.IsRange;

                await _blockedPermittedIpAddressesCommands.UpdateAsync(currentSiteId.ToString(), blockedPermittedIpAddressesModel.Id.ToString(), entity).ConfigureAwait(false);

                return true;
            }
            catch (Exception e)
            {
                _log.LogError($"Error updating permitted IP Address");
                return false;
            }
        }

        public async Task<bool> DeletePermittedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (id == Guid.Empty || siteId == Guid.Empty)
            {
                _log.LogError("ID or Site ID cannot be empty");
                throw new ArgumentException("ID or Site ID cannot be empty");
            }

            try
            {
                IEnumerable<BlockedPermittedIpAddressesModel> results = await _blockedPermittedIpAddressesQueries.GetAllAsync(siteId.ToString(), cancellationToken).ConfigureAwait(false);

                BlockedPermittedIpAddressesModel ipAddress = results.Where(x => x.Id == id && x.SiteId == siteId && x.IsPermitted == true).FirstOrDefault();

                if (ipAddress != null)
                {

                    await _blockedPermittedIpAddressesCommands.DeleteAsync(siteId.ToString(), ipAddress.Id.ToString()).ConfigureAwait(false);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _log.LogError($"Error updating permitted IP Address");
                return false;
            }
        }

        public async Task<PagedResult<BlockedPermittedIpAddressesModel>> SearchPermittedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0) throw new ArgumentNullException("must include site ID");

            int offset = (pageSize * pageNumber) - pageSize;

            IEnumerable<BlockedPermittedIpAddressesModel> results = await _blockedPermittedIpAddressesQueries.GetAllAsync(siteId.ToString(), cancellationToken).ConfigureAwait(false);

            IEnumerable<BlockedPermittedIpAddressesModel> data = results.OrderBy
                            (x => x.CreatedDate)
                            .Where(s => s.SiteId == siteId && s.IsPermitted == true)
                            .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                            .Skip(offset)
                            .Take(pageSize);


            PagedResult<BlockedPermittedIpAddressesModel> result = new PagedResult<BlockedPermittedIpAddressesModel>();

            result.Data = data.Where(x => x.SiteId == siteId && x.IsPermitted == true).ToList();
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = results.Where(s => s.SiteId == siteId && s.IsPermitted == true)
                    .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                    .Count();

            return result;
        }


        public async Task<PagedResult<BlockedPermittedIpAddressesModel>> GetBlockedIpAddresses(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken), bool? IsFromService = false)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0)
            {
                _log.LogError("Site ID must be provided to retrieve blocked IP addresses");
                throw new ArgumentNullException("Site ID is required");
            }

            int offset = (pageSize * pageNumber) - pageSize;
            IQueryable<BlockedPermittedIpAddressesModel> query;

            IEnumerable<BlockedPermittedIpAddressesModel> data;
            IEnumerable<BlockedPermittedIpAddressesModel> results = await _blockedPermittedIpAddressesQueries.GetAllAsync(siteId.ToString(), cancellationToken).ConfigureAwait(false);

            if ((bool)IsFromService)
            {
                data = results.OrderBy
                                (x => x.CreatedDate)
                                .Where(s => s.SiteId == siteId && s.IsPermitted == false)
                                .Skip(offset);
            }
            else
            {
                data = results.OrderBy
                                (x => x.CreatedDate)
                                .Where(s => s.SiteId == siteId && s.IsPermitted == false)
                                .Skip(offset)
                                .Take(pageSize);
            }

            PagedResult<BlockedPermittedIpAddressesModel> result = new PagedResult<BlockedPermittedIpAddressesModel>();

            result.Data = data.Where(x => x.SiteId == siteId && x.IsPermitted == false).ToList();
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;

            result.TotalItems = results.Where(s => s.SiteId == siteId && s.IsPermitted == false).Count();

            return result;
        }

        public async Task<bool> AddBlockedIpAddress(BlockedPermittedIpAddressesModel blockedPermittedIpAddressesModel, Guid currentSiteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (blockedPermittedIpAddressesModel == null)
            {
                _log.LogError($"IP Address Model is required");
                throw new ArgumentException($"IP Address Model is required");
            }
            if (blockedPermittedIpAddressesModel.IpAddress.Length <= 0)
            {
                _log.LogError($"IP Address is required");
                throw new ArgumentException($"IP Address is required");
            }
            if (blockedPermittedIpAddressesModel.SiteId.ToString() == null)
            {
                _log.LogError($"Site ID is required");
                throw new ArgumentException($"Site ID is required");
            }

            try
            {
                await _blockedPermittedIpAddressesCommands.CreateAsync(currentSiteId.ToString(), blockedPermittedIpAddressesModel.Id.ToString(), blockedPermittedIpAddressesModel).ConfigureAwait(false);

                return true;
            }
            catch (Exception e)
            {
                _log.LogError($"Error creating blocked IP Address");
                return false;
            }
        }

        public async Task<bool> UpdateBlockedIpAddress(BlockedPermittedIpAddressesModel blockedPermittedIpAddressesModel, Guid currentSiteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (blockedPermittedIpAddressesModel == null)
            {
                _log.LogError($"IP Address Model is required");
                throw new ArgumentException($"IP Address Model is required");
            }
            if (blockedPermittedIpAddressesModel.IpAddress.Length <= 0)
            {
                _log.LogError($"IP Address is required");
                throw new ArgumentException($"IP Address is required");
            }
            if (blockedPermittedIpAddressesModel.SiteId.ToString() == null)
            {
                _log.LogError($"Site ID is required");
                throw new ArgumentException($"Site ID is required");
            }

            try
            {
                IEnumerable<BlockedPermittedIpAddressesModel> results = await _blockedPermittedIpAddressesQueries.GetAllAsync(currentSiteId.ToString(), cancellationToken).ConfigureAwait(false);

                var entity = results.FirstOrDefault(x => x.Id == blockedPermittedIpAddressesModel.Id && x.SiteId == blockedPermittedIpAddressesModel.SiteId && blockedPermittedIpAddressesModel.IsPermitted == false);

                if (entity == null)
                    return false;

                entity.IpAddress = blockedPermittedIpAddressesModel.IpAddress;
                entity.Reason = blockedPermittedIpAddressesModel.Reason;
                entity.LastUpdated = blockedPermittedIpAddressesModel.LastUpdated;
                entity.IsRange = blockedPermittedIpAddressesModel.IsRange;

                await _blockedPermittedIpAddressesCommands.UpdateAsync(currentSiteId.ToString(), blockedPermittedIpAddressesModel.Id.ToString(), entity).ConfigureAwait(false);

                return true;
            }
            catch (Exception e)
            {
                _log.LogError($"Error updating blocked IP Address");
                return false;
            }
        }

        public async Task<bool> DeleteBlockedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (id == Guid.Empty || siteId == Guid.Empty)
            {
                _log.LogError("ID or Site ID cannot be empty");
                throw new ArgumentException("ID or Site ID cannot be empty");
            }

            try
            {
                IEnumerable<BlockedPermittedIpAddressesModel> results = await _blockedPermittedIpAddressesQueries.GetAllAsync(siteId.ToString(), cancellationToken).ConfigureAwait(false);

                BlockedPermittedIpAddressesModel ipAddress = results.Where(x => x.Id == id && x.SiteId == siteId && x.IsPermitted == false).FirstOrDefault();

                if (ipAddress != null)
                {
                    await _blockedPermittedIpAddressesCommands.DeleteAsync(siteId.ToString(), ipAddress.Id.ToString()).ConfigureAwait(false);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _log.LogError($"Error updating permitted IP Address");
                return false;
            }
        }

        public async Task<PagedResult<BlockedPermittedIpAddressesModel>> SearchBlockedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0) throw new ArgumentNullException("must include site ID");

            int offset = (pageSize * pageNumber) - pageSize;

            IEnumerable<BlockedPermittedIpAddressesModel> results = await _blockedPermittedIpAddressesQueries.GetAllAsync(siteId.ToString(), cancellationToken).ConfigureAwait(false);

            IEnumerable<BlockedPermittedIpAddressesModel> data = results.OrderBy
                            (x => x.CreatedDate)
                            .Where(s => s.SiteId == siteId && s.IsPermitted == false)
                            .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                            .Skip(offset)
                            .Take(pageSize);


            PagedResult<BlockedPermittedIpAddressesModel> result = new PagedResult<BlockedPermittedIpAddressesModel>();

            result.Data = data.Where(x => x.SiteId == siteId && x.IsPermitted == false).ToList();
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;
            result.TotalItems = results.Where(s => s.SiteId == siteId && s.IsPermitted == false)
                    .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                    .Count();

            return result;
        }
    }
}