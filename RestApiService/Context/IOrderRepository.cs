using RestApiService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiService.Context
{
    public interface IOrderRepository : IBasicRepository<Order>
    {
        IEnumerable<Order> GetFilteredOrdersWithSorting(int clientId, DateTime dateFrom, DateTime dateTo, string order);
    }
}
