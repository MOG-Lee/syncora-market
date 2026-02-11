using Microsoft.EntityFrameworkCore;
using Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using Syncora.Services.Ordering.Infrastructure.Persistence;
using Syncora.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingDbContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public OrderRepository(OrderingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Order Add(Order order)
        {
            return _context.Orders.Add(order).Entity;
        }

        // In DDD, we don't use Disconected scenario, we use EF to track changes
        //public void Update(Order order)
        //{
        //    _context.Entry(order).State = EntityState.Modified;
        //}

        public async Task<Order?> GetAsync(Guid orderId)
        {
            // Eager load 
            return await _context.Orders.Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
