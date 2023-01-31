using Microsoft.Extensions.Caching.Memory;
using RestApiService.Helpers;
using RestApiService.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestApiService.Context.Repo
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly EFDbContext _context;
        private readonly ISortHelper<Order> _sortHelper;
        private readonly IMemoryCache _cache;

        public EFOrderRepository(ISortHelper<Order> sortHelper, EFDbContext context, IMemoryCache cache)
        {
            _sortHelper = sortHelper;
            _context = context;
            _cache = cache;
        }

        public IEnumerable<Order> GetFilteredOrdersWithSorting(int clientId, DateTime dateFrom, DateTime dateTo, string order)
        {
            var orders = _context.Order.Where(p => p.ClientId == clientId && p.Date>dateFrom && p.Date<dateTo);
            return _sortHelper.ApplySort(orders, $"Date {order}");
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Order;
        }

        public void Create(Order order)
        {
            _context.Order.Add(order);
            _context.SaveChanges();
        }

        public Order Get(int id)
        {
            return _context.Order.Find(id);
        }

        public void Update(Order updatedOrder)
        {
            var order = Get(updatedOrder.Id);
            order.ClientId = updatedOrder.ClientId;
            order.Date = updatedOrder.Date;

            _context.Order.Update(order);
            _context.SaveChanges();
        }

        public Order Delete(int id)
        {
            var order = Get(id);

            if (order != null)
            {
                _context.Order.Remove(order);
                _context.SaveChanges();
            }

            return order;
        }

        public Order GetCache(int id)
        {
            _cache.TryGetValue(id, out Order order);
            if (order == null)
            {
                order = _context.Order.Find(id);
                if (order != null)
                {
                    _cache.Set(order.Id, order, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                    return _context.Order.Find(id);
                }
            }
            return order;
        }
    }
}
