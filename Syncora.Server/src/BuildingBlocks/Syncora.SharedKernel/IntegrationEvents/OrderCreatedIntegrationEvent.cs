using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.SharedKernel.IntegrationEvents
{
    public record OrderCreatedIntegrationEvent(Guid OrderId, Guid BuyerId, decimal TotalPrice,
        List<OrderItemDto> items);

    public record OrderItemDto(Guid ProductId, int Units);
}
