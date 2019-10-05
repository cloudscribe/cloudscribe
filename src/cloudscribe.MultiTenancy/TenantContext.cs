using System;
using System.Collections.Generic;

namespace cloudscribe.Multitenancy
{
	public class TenantContext<TTenant> : IDisposable
    {
        private bool disposed;

        public TenantContext(TTenant tenant)
        {
            Ensure.Argument.NotNull(tenant, nameof(Tenant));

            Tenant = tenant;
            Properties = new Dictionary<string, object>();
        }

        public string Id { get; } = Guid.NewGuid().ToString();
        public TTenant Tenant { get; }
        public IDictionary<string, object> Properties { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                foreach (var prop in Properties)
                {
                    TryDisposeProperty(prop.Value as IDisposable);
                }

				TryDisposeProperty(Tenant as IDisposable);
			}

            disposed = true;
        }

        private void TryDisposeProperty(IDisposable obj)
        {
            if (obj == null)
            {
                return;
            }

            try
            {
                obj.Dispose();
            }
            catch (ObjectDisposedException) { }
        }
    }
}
