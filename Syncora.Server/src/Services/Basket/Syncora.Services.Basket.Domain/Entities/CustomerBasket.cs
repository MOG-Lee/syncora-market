using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Basket.Domain.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket() { }

        public CustomerBasket(Guid buyerId)
        {
            BuyerId = buyerId;
        }

        public Guid BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new();

        public decimal TotalPrice => Items.Sum(item => item.UnitPrice * item.Quantity);
    }
}
