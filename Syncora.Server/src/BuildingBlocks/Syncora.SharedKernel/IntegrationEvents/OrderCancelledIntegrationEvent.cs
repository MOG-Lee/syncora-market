using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.SharedKernel.IntegrationEvents
{
    public record OrderCancelledIntegrationEvent(Guid OrderId, List<OrderCancelledItemDto> items);

    public record OrderCancelledItemDto(Guid ProductId, int Units);
}
