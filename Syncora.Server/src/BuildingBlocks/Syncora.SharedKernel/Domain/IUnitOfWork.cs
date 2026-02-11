using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.SharedKernel.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        // Standard EF Core commit
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatches Domain Events FIRST, then commits. 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    }
}
