using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Orders.Queries.GetOrderDetails
{
    public record GetOrderDetailsQuery(Guid OrderId) : IRequest<OrderDetailDto>;

    public record OrderDetailDto
    {
        public Guid OrderId { get; init; }
        public DateTime Date { get; init; }
        public string Status { get; init; } = default!;
        public decimal Total { get; init; }
        public string Street { get; init; } = default!;
        public string City { get; init; } = default!;
        public string ZipCode { get; init; } = default!;

        public IEnumerable<OrderViewItemDto> OrderItems { get; init; } = new List<OrderViewItemDto>();
    }

    public record OrderViewItemDto
    {
        public string ProductName { get; init; } = default!;
        public string PictureUrl { get; init; } = default!;
        public int Units { get; init; }
        public decimal UnitPrice { get; init; }
    }
}
