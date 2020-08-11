using System.Collections.Generic;
using System.Threading.Tasks;

namespace TennisBookings.Web.External
{
    public interface IProductsApiClient
    {
        Task<ProductsApiResult> GetProducts();
    }
    public class Product
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
    public class ProductsApiResult
    {
        public int TotalProducts { get; set; }

        public IReadOnlyCollection<Product> Products { get; set; }
    }
}
