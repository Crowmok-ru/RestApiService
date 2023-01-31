using Microsoft.Extensions.Caching.Memory;
using RestApiService.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestApiService.Context.Repo
{
    public class EFOrderPositionsRepository : IOrderPositionsRepository
    {
        private readonly EFDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;

        public EFOrderPositionsRepository(EFDbContext context, IOrderRepository orderRepository, IProductRepository productRepository, IMemoryCache cache)
        {
            _context = context;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _cache = cache;
        }

        public IEnumerable<OrderPositions> GetAll()
        {
            return _context.OrderPositions.ToList();            
        }

        public OrderPositions Get(int Id)
        {
            return _context.OrderPositions.Find(Id);
        }

        public void CreateOrder(int clientId, OrderPositions orderPositions)
        {
            var client = _context.Client.FirstOrDefault(c => c.Id == clientId);
            if (client == null)
            {
                throw new Exception("Данный клиент не найден. Проверьте правильность введенного ID или создайте нового клиента.");
            }

            var product = _context.Product.FirstOrDefault(p => p.Id == orderPositions.ProductId);
            if (product == null)
            {
                throw new Exception("Данный продукт не найден.Проверьте правильность введеного ID или создайте новый продукт.");
            }
            else
            {
                if (product.QuantityInStock < orderPositions.Quantity)
                {
                    throw new Exception("На складе нет необходимого количества продукта");
                }
            }

            _orderRepository.Create(new Order()
            {
                ClientId = clientId,
                Date = DateTime.Now
            });

            product.QuantityInStock -= orderPositions.Quantity; 
            _productRepository.Update(product);

            var newOrderId = _context.Order.ToList().Last().Id;
            orderPositions.OrderId = newOrderId;

            orderPositions.Price = orderPositions.Quantity * product.Price;

            _context.OrderPositions.Add(orderPositions);            
            _context.SaveChanges();
        }

        public void Create(OrderPositions item)
        {
            throw new NotImplementedException();
        }

        public void Update(OrderPositions updatedOrderPosition)
        {
            throw new NotImplementedException();
        }

        public OrderPositions Delete(int id)
        {
            var orderPositions = Get(id);

            if (orderPositions != null)
            {
                _context.OrderPositions.Remove(orderPositions);
                _context.SaveChanges();
            }

            return orderPositions;
        }

        public OrderPositions GetCache(int id)
        {
            _cache.TryGetValue(id, out OrderPositions orderPosition);
            if (orderPosition == null)
            {
                orderPosition = _context.OrderPositions.Find(id);
                if (orderPosition != null)
                {
                    _cache.Set(orderPosition.Id, orderPosition, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                    return _context.OrderPositions.Find(id);
                }
            }
            return orderPosition;
        }
    }
}
