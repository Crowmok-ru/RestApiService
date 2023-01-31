using RestApiService.Model;
using System.Collections.Generic;

namespace RestApiService.Context
{
    public interface IProductRepository : IBasicRepository<Product>
    {
        IEnumerable<Product> GetFilteredProductsWithSorting(int productTypeId, bool isStockAvailability, string order);
    }
}
