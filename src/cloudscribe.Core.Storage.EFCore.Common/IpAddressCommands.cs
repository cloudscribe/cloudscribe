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

        public async Task<PagedResult<BlackWhiteListedIpAddressesModel>> GetWhitelistedIpAddresses(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken), bool? IsFromService = false)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0) throw new ArgumentNullException("must include site ID");

            int offset = (pageSize * pageNumber) - pageSize;
            IQueryable<BlackWhiteListedIpAddressesModel> query;

            using (ICoreDbContext dbContext = _contextFactory.CreateContext())
            {
                if ((bool)IsFromService)
                {
                    query = dbContext.BlackWhiteListedIpAddresses.OrderBy
                                (x => x.CreatedDate)
                                .Where(s => s.SiteId == siteId && s.IsWhitelisted == true)
                                .Skip(offset);
                }
                else
                {
                    query = dbContext.BlackWhiteListedIpAddresses.OrderBy
                                (x => x.CreatedDate)
                                .Where(s => s.SiteId == siteId && s.IsWhitelisted == true)
                                .Skip(offset)
                                .Take(pageSize);
                }
                var data = await query
                    .AsNoTracking()
                    .ToListAsync<BlackWhiteListedIpAddressesModel>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<BlackWhiteListedIpAddressesModel>();

                result.Data = data.Where(x => x.SiteId == siteId && x.IsWhitelisted == true).ToList();
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await dbContext.BlackWhiteListedIpAddresses
                    .Where(s => s.SiteId == siteId && s.IsWhitelisted == true)
                    .CountAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            };
        }

        public async Task<bool> AddWhitelistedIpAddress(BlackWhiteListedIpAddressesModel blackWhiteListedIpAddressesModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (blackWhiteListedIpAddressesModel == null)
            {
                _log.LogError($"IP Address Model is required");
                throw new ArgumentException($"IP Address Model is required");
            }
            if (blackWhiteListedIpAddressesModel.IpAddress.Length <= 0)
            {
                _log.LogError($"IP Address is required");
                throw new ArgumentException($"IP Address is required");
            }
            if (blackWhiteListedIpAddressesModel.SiteId.ToString() == null)
            {
                _log.LogError($"Site ID is required");
                throw new ArgumentException($"Site ID is required");
            }

            int rowsAffected = 0;

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.BlackWhiteListedIpAddresses.Add(blackWhiteListedIpAddressesModel);

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

        public async Task<bool> UpdateWhitelistedIpAddress(BlackWhiteListedIpAddressesModel blackWhiteListedIpAddressesModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (blackWhiteListedIpAddressesModel == null)
            {
                _log.LogError($"IP Address Model is required");
                throw new ArgumentException($"IP Address Model is required");
            }
            if (blackWhiteListedIpAddressesModel.IpAddress.Length <= 0)
            {
                _log.LogError($"IP Address is required");
                throw new ArgumentException($"IP Address is required");
            }
            if (blackWhiteListedIpAddressesModel.SiteId.ToString() == null)
            {
                _log.LogError($"Site ID is required");
                throw new ArgumentException($"Site ID is required");
            }

            int rowsAffected = 0;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var entity = await dbContext.BlackWhiteListedIpAddresses
                    .FirstOrDefaultAsync(x => x.Id == blackWhiteListedIpAddressesModel.Id && x.SiteId == blackWhiteListedIpAddressesModel.SiteId && blackWhiteListedIpAddressesModel.IsWhitelisted == true, cancellationToken);

                if (entity == null)
                    return false;

                entity.IpAddress = blackWhiteListedIpAddressesModel.IpAddress;
                entity.Reason = blackWhiteListedIpAddressesModel.Reason;
                entity.LastUpdated = blackWhiteListedIpAddressesModel.LastUpdated;

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

        public async Task<bool> DeleteWhitelistedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (id == Guid.Empty || siteId == Guid.Empty)
            {
                _log.LogError("ID or Site ID cannot be empty");
                throw new ArgumentException("ID or Site ID cannot be empty");
            }

            using (var dbContext = _contextFactory.CreateContext())
            {
                var ipAddress = await dbContext.BlackWhiteListedIpAddresses
                    .Where(s => s.Id == id && s.SiteId == siteId && s.IsWhitelisted == true)
                    .FirstOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (ipAddress != null)
                {
                    dbContext.BlackWhiteListedIpAddresses.Remove(ipAddress);
                    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<PagedResult<BlackWhiteListedIpAddressesModel>> SearchWhitelistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0) throw new ArgumentNullException("must include site ID");

            int offset = (pageSize * pageNumber) - pageSize;
            IQueryable<BlackWhiteListedIpAddressesModel> query;

            using (ICoreDbContext dbContext = _contextFactory.CreateContext())
            {
                query = dbContext.BlackWhiteListedIpAddresses.OrderBy
                            (x => x.CreatedDate)
                            .Where(s => s.SiteId == siteId && s.IsWhitelisted == true)
                            .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                            .Skip(offset)
                            .Take(pageSize);

                var data = await query
                    .AsNoTracking()
                    .ToListAsync<BlackWhiteListedIpAddressesModel>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<BlackWhiteListedIpAddressesModel>();

                result.Data = data.Where(x => x.SiteId == siteId && x.IsWhitelisted == true).ToList();
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await dbContext.BlackWhiteListedIpAddresses
                    .Where(s => s.SiteId == siteId && s.IsWhitelisted == true)
                    .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                    .CountAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            };
        }


        public async Task<PagedResult<BlackWhiteListedIpAddressesModel>> GetBlacklistedIpAddresses(Guid siteId, int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken), bool? IsFromService = false)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0)
            {
                _log.LogError("Site ID must be provided to retrieve blacklisted IP addresses");
                throw new ArgumentNullException("Site ID is required");
            }

            int offset = (pageSize * pageNumber) - pageSize;
            IQueryable<BlackWhiteListedIpAddressesModel> query;

            using (ICoreDbContext dbContext = _contextFactory.CreateContext())
            {
                if ((bool)IsFromService)
                {
                    query = dbContext.BlackWhiteListedIpAddresses.OrderBy
                                (x => x.CreatedDate)
                                .Where(s => s.SiteId == siteId && s.IsWhitelisted == false)
                                .Skip(offset);
                }
                else
                {
                    query = dbContext.BlackWhiteListedIpAddresses.OrderBy
                                (x => x.CreatedDate)
                                .Where(s => s.SiteId == siteId && s.IsWhitelisted == false)
                                .Skip(offset)
                                .Take(pageSize);
                }
                var data = await query
                    .AsNoTracking()
                    .ToListAsync<BlackWhiteListedIpAddressesModel>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<BlackWhiteListedIpAddressesModel>();

                result.Data = data.Where(x => x.SiteId == siteId && x.IsWhitelisted == false).ToList();
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await dbContext.BlackWhiteListedIpAddresses
                    .Where(s => s.SiteId == siteId && s.IsWhitelisted == false)
                    .CountAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            };
        }

        public async Task<bool> AddBlacklistedIpAddress(BlackWhiteListedIpAddressesModel blackWhiteListedIpAddressesModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (blackWhiteListedIpAddressesModel == null)
            {
                _log.LogError($"IP Address Model is required");
                throw new ArgumentException($"IP Address Model is required");
            }
            if (blackWhiteListedIpAddressesModel.IpAddress.Length <= 0)
            {
                _log.LogError($"IP Address is required");
                throw new ArgumentException($"IP Address is required");
            }
            if (blackWhiteListedIpAddressesModel.SiteId.ToString() == null)
            {
                _log.LogError($"Site ID is required");
                throw new ArgumentException($"Site ID is required");
            }

            int rowsAffected = 0;

            using (var dbContext = _contextFactory.CreateContext())
            {
                dbContext.BlackWhiteListedIpAddresses.Add(blackWhiteListedIpAddressesModel);

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

        public async Task<bool> UpdateBlacklistedIpAddress(BlackWhiteListedIpAddressesModel blackWhiteListedIpAddressesModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (blackWhiteListedIpAddressesModel == null)
            {
                _log.LogError($"IP Address Model is required");
                throw new ArgumentException($"IP Address Model is required");
            }
            if (blackWhiteListedIpAddressesModel.IpAddress.Length <= 0)
            {
                _log.LogError($"IP Address is required");
                throw new ArgumentException($"IP Address is required");
            }
            if (blackWhiteListedIpAddressesModel.SiteId.ToString() == null)
            {
                _log.LogError($"Site ID is required");
                throw new ArgumentException($"Site ID is required");
            }

            int rowsAffected = 0;

            using (var dbContext = _contextFactory.CreateContext())
            {
                var entity = await dbContext.BlackWhiteListedIpAddresses
                    .FirstOrDefaultAsync(x => x.Id == blackWhiteListedIpAddressesModel.Id && x.SiteId == blackWhiteListedIpAddressesModel.SiteId && blackWhiteListedIpAddressesModel.IsWhitelisted == false, cancellationToken);

                if (entity == null)
                    return false;

                entity.IpAddress = blackWhiteListedIpAddressesModel.IpAddress;
                entity.Reason = blackWhiteListedIpAddressesModel.Reason;
                entity.LastUpdated = blackWhiteListedIpAddressesModel.LastUpdated;

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

        public async Task<bool> DeleteBlacklistedIpAddress(Guid id, Guid siteId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (id == Guid.Empty || siteId == Guid.Empty)
            {
                _log.LogError("ID or Site ID cannot be empty");
                throw new ArgumentException("ID or Site ID cannot be empty");
            }

            using (var dbContext = _contextFactory.CreateContext())
            {
                var ipAddress = await dbContext.BlackWhiteListedIpAddresses
                    .Where(s => s.Id == id && s.SiteId == siteId && s.IsWhitelisted == false)
                    .FirstOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);

                if (ipAddress != null)
                {
                    dbContext.BlackWhiteListedIpAddresses.Remove(ipAddress);
                    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<PagedResult<BlackWhiteListedIpAddressesModel>> SearchBlacklistedIpAddressesAsync(Guid siteId, int pageNumber, int pageSize, string searchTerm, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (siteId.ToString().Length <= 0) throw new ArgumentNullException("must include site ID");

            int offset = (pageSize * pageNumber) - pageSize;
            IQueryable<BlackWhiteListedIpAddressesModel> query;

            using (ICoreDbContext dbContext = _contextFactory.CreateContext())
            {
                query = dbContext.BlackWhiteListedIpAddresses.OrderBy
                            (x => x.CreatedDate)
                            .Where(s => s.SiteId == siteId && s.IsWhitelisted == false)
                            .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                            .Skip(offset)
                            .Take(pageSize);

                var data = await query
                    .AsNoTracking()
                    .ToListAsync<BlackWhiteListedIpAddressesModel>(cancellationToken)
                    .ConfigureAwait(false);

                var result = new PagedResult<BlackWhiteListedIpAddressesModel>();

                result.Data = data.Where(x => x.SiteId == siteId && x.IsWhitelisted == false).ToList();
                result.PageNumber = pageNumber;
                result.PageSize = pageSize;
                result.TotalItems = await dbContext.BlackWhiteListedIpAddresses
                    .Where(s => s.SiteId == siteId && s.IsWhitelisted == false)
                    .Where(s => s.IpAddress.Contains(searchTerm.ToLower()) || (s.Reason != null && s.Reason.Contains(searchTerm.ToLower())))
                    .CountAsync(cancellationToken)
                    .ConfigureAwait(false);

                return result;
            };
        }
    }
}
