using Syncora.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public interface IOrderRepository
    {
        IUnitOfWork UnitOfWork { get; }
        Order Add(Order order);
        Task<Order?> GetAsync(Guid orderId);
    }
}
