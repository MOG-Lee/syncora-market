using MediatR;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Application.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(Guid BuyerId, CreateOrderAddressDto ShippingAddress,
        List<CreateOrderOrderItemDto> OrderItems) : IRequest<Guid>;


    public record CreateOrderOrderItemDto(Guid ProductId, string ProductName, decimal UnitPrice, string PictureUrl, int Units);
    public record CreateOrderAddressDto(string Street, string City, string ZipCode);
}
