using Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using Syncora.SharedKernel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Domain.Events
{
    public record OrderStartedDomainEvent(Order Order) : DomainEvent;
}
