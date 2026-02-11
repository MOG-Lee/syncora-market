using MediatR;
using Syncora.Services.Ordering.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Orders.Queries.GetOrderSummary
{
    public class GetOrderSummaryQueryHandler : IRequestHandler<GetOrderSummaryQuery, IEnumerable<OrderSummaryDto>>
    {
        private readonly IOrderQueryService _orderQueryService;

        public GetOrderSummaryQueryHandler(IOrderQueryService orderQueryService)
        {
            _orderQueryService = orderQueryService;
        }

        public async Task<IEnumerable<OrderSummaryDto>> Handle(GetOrderSummaryQuery request, CancellationToken cancellationToken)
        {
            return await _orderQueryService.GetOrdersFromUserAsync(request.UserId);
        }
    }
}
