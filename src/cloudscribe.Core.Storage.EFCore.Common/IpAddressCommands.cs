using cloudscribe.Core.Models;
using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.Storage.EFCore.Common
{
    public class IpAddressCommands : IipAddressCommands
    {
        private readonly ICoreDbContextFactory _contextFactory;
        private ILogger _log;

        public IpAddressCommands(ICoreDbContextFactory coreDbContextFactory, ILogger<IpAddressCommands> logger)
        {
            _contextFactory = coreDbContextFactory;
            _log = logger;
        }

        public async Task<PagedResult<BlockedPermittedIpAddressesModel>> GetPermittedIpAddresses(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken), bool? IsFromService = false)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0) throw new ArgumentNullException("must include site ID");

            int offset = (pageSize * pageNumber) - pageSize;
            IQueryable<BlockedPermittedIpAddressesModel> query;

            using (ICoreDbContext dbContext = _contextFactory.CreateContext())
            {
                if ((bool)IsFromService)
                {
                    query = dbContext.BlockedPermittedIpAddresses.OrderBy
                                (x => x.CreatedDate)
                                .AsSingleQuery()
                                .Where(s => s.SiteId == siteId && s.IsPermitted == true)
                                .Skip(offset);
                }
                else
                {
                    query = dbContext.BlockedPermittedIpAddresses.OrderBy
                                (x => x.CreatedDate)
                                .AsSingleQuery()
                                .Where(s => s.SiteId == siteId && s.IsPermitted == true)
                                .Skip(offset)
                                .Take(pageSize);
                }
                var data = await query
                    .AsNoTracking()
                    .ToListAsync<BlockedPermittedIpAddressesModel>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<BlockedPermittedIpAddressesModel>();

                result.Data = data.Where(x => x.SiteId == siteId && x.IsPermitted == true).ToList();
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await dbContext.BlockedPermittedIpAddresses
                    .Where(s => s.SiteId == siteId && s.IsPermitted == true)
                    .CountAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            };
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

            int rowsAffected = 0;

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.BlockedPermittedIpAddresses.Add(blockedPermittedIpAddressesModel);

                rowsAffected = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
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
            if (currentSiteId.ToString() == null)
            {
                _log.LogError($"Site ID is required");
                throw new ArgumentException($"Site ID is required");
            }

            int rowsAffected = 0;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var entity = await dbContext.BlockedPermittedIpAddresses
                    .FirstOrDefaultAsync(x => x.Id == blockedPermittedIpAddressesModel.Id && x.SiteId == currentSiteId && blockedPermittedIpAddressesModel.IsPermitted == true, cancellationToken);

                if (entity == null)
                    return false;

                entity.IpAddress = blockedPermittedIpAddressesModel.IpAddress;
                entity.Reason = blockedPermittedIpAddressesModel.Reason;
                entity.LastUpdated = blockedPermittedIpAddressesModel.LastUpdated;
                entity.IsRange = blockedPermittedIpAddressesModel.IsRange;

                rowsAffected = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
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

            using (var dbContext = _contextFactory.CreateContext())
            {
                var ipAddress = await dbContext.BlockedPermittedIpAddresses
                    .Where(s => s.Id == id && s.SiteId == siteId && s.IsPermitted == true)
                    .FirstOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (ipAddress != null)
                {
                    dbContext.BlockedPermittedIpAddresses.Remove(ipAddress);
                    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<PagedResult<BlockedPermittedIpAddressesModel>> SearchPermittedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0) throw new ArgumentNullException("must include site ID");

            int offset = (pageSize * pageNumber) - pageSize;
            IQueryable<BlockedPermittedIpAddressesModel> query;

            using (ICoreDbContext dbContext = _contextFactory.CreateContext())
            {
                query = dbContext.BlockedPermittedIpAddresses.OrderBy
                            (x => x.CreatedDate)
                            .AsSingleQuery()
                            .Where(s => s.SiteId == siteId && s.IsPermitted == true)
                            .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                            .Skip(offset)
                            .Take(pageSize);

                var data = await query
                    .AsNoTracking()
                    .ToListAsync<BlockedPermittedIpAddressesModel>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<BlockedPermittedIpAddressesModel>();

                result.Data = data.Where(x => x.SiteId == siteId && x.IsPermitted == true).ToList();
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await dbContext.BlockedPermittedIpAddresses
                    .Where(s => s.SiteId == siteId && s.IsPermitted == true)
                    .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                    .CountAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            };
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

            using (ICoreDbContext dbContext = _contextFactory.CreateContext())
            {
                if ((bool)IsFromService)
                {
                    query = dbContext.BlockedPermittedIpAddresses.OrderBy
                                (x => x.CreatedDate)
                                .AsSingleQuery()
                                .Where(s => s.SiteId == siteId && s.IsPermitted == false)
                                .Skip(offset);
                }
                else
                {
                    query = dbContext.BlockedPermittedIpAddresses.OrderBy
                                (x => x.CreatedDate)
                                .AsSingleQuery()
                                .Where(s => s.SiteId == siteId && s.IsPermitted == false)
                                .Skip(offset)
                                .Take(pageSize);
                }
                var data = await query
                    .AsNoTracking()
                    .ToListAsync<BlockedPermittedIpAddressesModel>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<BlockedPermittedIpAddressesModel>();

                result.Data = data.Where(x => x.SiteId == siteId && x.IsPermitted == false).ToList();
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await dbContext.BlockedPermittedIpAddresses
                    .Where(s => s.SiteId == siteId && s.IsPermitted == false)
                    .CountAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            };
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

            int rowsAffected = 0;

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.BlockedPermittedIpAddresses.Add(blockedPermittedIpAddressesModel);

                rowsAffected = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
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
            if (currentSiteId.ToString() == null)
            {
                _log.LogError($"Site ID is required");
                throw new ArgumentException($"Site ID is required");
            }

            int rowsAffected = 0;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var entity = await dbContext.BlockedPermittedIpAddresses
                    .FirstOrDefaultAsync(x => x.Id == blockedPermittedIpAddressesModel.Id && x.SiteId == currentSiteId && blockedPermittedIpAddressesModel.IsPermitted == false, cancellationToken);

                if (entity == null)
                    return false;

                entity.IpAddress = blockedPermittedIpAddressesModel.IpAddress;
                entity.Reason = blockedPermittedIpAddressesModel.Reason;
                entity.LastUpdated = blockedPermittedIpAddressesModel.LastUpdated;
                entity.IsRange = blockedPermittedIpAddressesModel.IsRange;

                rowsAffected = await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
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

            using (var dbContext = _contextFactory.CreateContext())
            {
                var ipAddress = await dbContext.BlockedPermittedIpAddresses
                    .Where(s => s.Id == id && s.SiteId == siteId && s.IsPermitted == false)
                    .FirstOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (ipAddress != null)
                {
                    dbContext.BlockedPermittedIpAddresses.Remove(ipAddress);
                    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<PagedResult<BlockedPermittedIpAddressesModel>> SearchBlockedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0) throw new ArgumentNullException("must include site ID");

            int offset = (pageSize * pageNumber) - pageSize;
            IQueryable<BlockedPermittedIpAddressesModel> query;

            using (ICoreDbContext dbContext = _contextFactory.CreateContext())
            {
                query = dbContext.BlockedPermittedIpAddresses.OrderBy
                            (x => x.CreatedDate)
                            .AsSingleQuery()
                            .Where(s => s.SiteId == siteId && s.IsPermitted == false)
                            .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                            .Skip(offset)
                            .Take(pageSize);

                var data = await query
                    .AsNoTracking()
                    .ToListAsync<BlockedPermittedIpAddressesModel>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<BlockedPermittedIpAddressesModel>();

                result.Data = data.Where(x => x.SiteId == siteId && x.IsPermitted == false).ToList();
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await dbContext.BlockedPermittedIpAddresses
                    .Where(s => s.SiteId == siteId && s.IsPermitted == false)
                    .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                    .CountAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            };
        }
    }
}
