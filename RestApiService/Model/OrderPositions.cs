using System.Text.Json.Serialization;

namespace RestApiService.Model
{
    
    public class OrderPositions : IEntity
    {
        public int Id { get; set; }        
        public int OrderId { get; set; }        
        public int ProductId { get; set; }        
        public decimal Price { get; set; }        
        public int Quantity { get; set; }
    }
}
