using Microsoft.Extensions.Caching.Memory;
using RestApiService.Helpers;
using RestApiService.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestApiService.Context.Repo
{
    public class EFProductRepository : IProductRepository
    {
        private readonly EFDbContext _context;
        private readonly ISortHelper<Product> _sortHelper;
        private readonly IMemoryCache _cache;

        public EFProductRepository(EFDbContext context, ISortHelper<Product> sortHelper, IMemoryCache cache)
        {
            _context = context;
            _sortHelper = sortHelper;
            _cache = cache;
        }

        public IEnumerable<Product> GetAll ()
        {
            return _context.Product;
        }

        public Product Get(int id)
        {
            return _context.Product.Find(id);
        }

        public void Update(Product updatedProduct)
        {
            Product product = Get(updatedProduct.Id);
            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            product.ProductTypeId = updatedProduct.ProductTypeId;
            product.QuantityInStock = updatedProduct.QuantityInStock;

            _context.Product.Update(product);
            _context.SaveChanges();
        }

        public IEnumerable<Product> GetFilteredProductsWithSorting(int productTypeId, bool isStockAvailability, string order)
        {
            var products = _context.Product.Where(p => p.ProductTypeId == productTypeId && ((isStockAvailability && p.QuantityInStock > 0) || (!isStockAvailability && p.QuantityInStock == 0)));
            return _sortHelper.ApplySort(products, $"Price {order}");
        }

        public void Create(Product item)
        {
            _context.Product.Add(item);
            _context.SaveChanges();
        }

        public Product Delete(int id)
        {
            var product = Get(id);

            if (product != null)
            {
                _context.Product.Remove(product);
                _context.SaveChanges();
            }

            return product;
        }

        public Product GetCache(int id)
        {
            _cache.TryGetValue(id, out Product product);
            if (product == null)
            {
                product = _context.Product.Find(id);
                if (product != null)
                {
                    _cache.Set(product.Id, product, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                    return _context.Product.Find(id);
                }
            }
            return product;
        }
    }
}
