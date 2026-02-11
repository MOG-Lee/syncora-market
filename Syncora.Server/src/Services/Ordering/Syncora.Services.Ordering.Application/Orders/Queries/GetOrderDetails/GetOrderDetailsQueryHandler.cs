using MediatR;
using Syncora.Services.Ordering.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Orders.Queries.GetOrderDetails
{
    public class GetOrderDetailsQueryHandler : IRequestHandler<GetOrderDetailsQuery, OrderDetailDto>
    {
        private readonly IOrderQueryService _orderQueryService;
        public GetOrderDetailsQueryHandler(IOrderQueryService orderQueryService)
        {
            _orderQueryService = orderQueryService;
        }

        public async Task<OrderDetailDto> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _orderQueryService.GetOrderAsync(request.OrderId);
        }
    }
}
