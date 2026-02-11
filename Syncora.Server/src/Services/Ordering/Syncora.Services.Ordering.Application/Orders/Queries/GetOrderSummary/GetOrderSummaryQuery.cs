using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Orders.Queries.GetOrderSummary
{
    public record GetOrderSummaryQuery(Guid UserId) : IRequest<IEnumerable<OrderSummaryDto>>;

    public record OrderSummaryDto
    {
        public Guid OrderId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = default!;
        public decimal Total { get; set; }
        public int TotalItems { get; set; }
    }
}
