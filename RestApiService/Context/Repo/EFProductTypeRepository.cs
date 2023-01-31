using Microsoft.Extensions.Caching.Memory;
using RestApiService.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestApiService.Context.Repo
{
    public class EFProductTypeRepository : IBasicRepository<ProductType>
    {
        private readonly EFDbContext _context;
        private readonly IMemoryCache _cache;

        public EFProductTypeRepository(EFDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public void Create(ProductType item)
        {
            _context.ProductType.Add(item);
            _context.SaveChanges();
        }

        public ProductType Delete(int id)
        {
            var productType = Get(id);

            if (productType != null)
            {
                var existForeignKey = _context.Product.Where(o => o.ProductTypeId == id);
                if (existForeignKey != null)
                {
                    var ids = string.Join(",", existForeignKey.Select(s => s.Id).ToList());
                    throw new Exception($"Удаление невозможно т.к. тип продукт используется в продуктах {ids}");
                }

                _context.ProductType.Remove(productType);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Тип продукта не найден");
            }

            return productType;
        }

        public IEnumerable<ProductType> GetAll()
        {
            return _context.ProductType;
        }

        public ProductType Get(int id)
        {
            return _context.ProductType.Find(id);
        }

        public void Update(ProductType updatedProductType)
        {
            var productType = Get(updatedProductType.Id);
            productType.Type = updatedProductType.Type;

            _context.ProductType.Update(productType);
            _context.SaveChanges();
        }

        public ProductType GetCache(int id)
        {
            _cache.TryGetValue(id, out ProductType productType);
            if (productType == null)
            {
                productType = _context.ProductType.Find(id);
                if (productType != null)
                {
                    _cache.Set(productType.Id, productType, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                    return _context.ProductType.Find(id);
                }
            }
            return productType;
        }
    }
}
