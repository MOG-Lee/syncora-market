using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public record OrderItemCandidate(Guid ProductId, string ProductName,
                    decimal UnitPrice, string PictureUrl, int Units);
}
