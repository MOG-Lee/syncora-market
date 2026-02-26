using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Catalog.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageFileName { get; set; } = string.Empty;
        public decimal Price { get; set; }

        //1. DYNAMIC ATTRIBUTES like "RAM", "Color"
        public Dictionary<string, string> Attributes { get; set; } = new();

        // 2. NESTED DOCUMENTS: This embeds a collection of comments directly inside the product
        public List<ProductComment> Comments { get; set; } = new();
    }

    // Create a simple class to represent the nested data
    public class ProductComment
    {
        public string UserName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
