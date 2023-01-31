using RestApiService.Model;
using System.Collections.Generic;

namespace RestApiService.Context
{
    public interface IOrderPositionsRepository : IBasicRepository<OrderPositions>
    {
        void CreateOrder(int clientId,OrderPositions orderPositions);
    }
}
