using System;

namespace RestApiService.Model
{
    public class Order : IEntity
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTime Date { get; set; }
    }
}
