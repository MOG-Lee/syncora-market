using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.SharedKernel.IntegrationEvents
{
    public record StockRejectedIntegrationEvent(Guid OrderId, string Reason);
}
