//// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
//// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using cloudscribe.Core.IdentityServer.EFCore.DbContexts;
//using cloudscribe.Core.IdentityServer.EFCore.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace cloudscribe.Core.IdentityServer.EFCore
//{
//    public class TokenCleanup
//    {
//        private readonly DbContextOptions<PersistedGrantDbContextBase> options;
//        private readonly TimeSpan interval;
//        private CancellationTokenSource source;

//        public TokenCleanup(DbContextOptions<PersistedGrantDbContextBase> options, int interval = 60)
//        {
//            if (options == null) throw new ArgumentNullException(nameof(options));
//            if (interval < 1) throw new ArgumentException("interval must be more than 1 second");
//            this.options = options;

//            this.interval = TimeSpan.FromSeconds(interval);
//        }

//        public void Start()
//        {
//            if (source != null) throw new InvalidOperationException("Already started. Call Stop first.");

//            source = new CancellationTokenSource();
//            Task.Factory.StartNew(() => Start(source.Token));
//        }

//        public void Stop()
//        {
//            if (source == null) throw new InvalidOperationException("Not started. Call Start first.");

//            source.Cancel();
//            source = null;
//        }

//        public async Task Start(CancellationToken cancellationToken)
//        {
//            while (true)
//            {
//                if (cancellationToken.IsCancellationRequested)
//                {
//                    //Logger.Info("CancellationRequested");
//                    break;
//                }

//                try
//                {
//                    await Task.Delay(interval, cancellationToken);
//                }
//                catch
//                {
//                    //Logger.Info("Task.Delay exception. exiting.");
//                    break;
//                }

//                if (cancellationToken.IsCancellationRequested)
//                {
//                    //Logger.Info("CancellationRequested");
//                    break;
//                }

//                await ClearTokens();
//            }
//        }

//        protected virtual IPersistedGrantDbContext CreateOperationalDbContext()
//        {
//            return new PersistedGrantDbContextBase(options);
//        }

//        private async Task ClearTokens()
//        {
//            try
//            {
//                //Logger.Info("Clearing tokens");

//                using (var context = CreateOperationalDbContext())
//                {
//                    var expired = context.PersistedGrants.Where(x => x.Expiration < DateTimeOffset.UtcNow);

//                    context.PersistedGrants.RemoveRange(expired);

//                    await context.SaveChangesAsync();
//                }
//            }
//            catch (Exception)
//            {
//                // TODO
//                //Logger.ErrorException("Exception cleaning tokens", exception);
//            }
//        }

//    }
//}