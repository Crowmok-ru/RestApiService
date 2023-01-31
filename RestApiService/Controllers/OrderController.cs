using Microsoft.AspNetCore.Mvc;
using RestApiService.Context;
using RestApiService.Model;
using System;
using System.Collections.Generic;

namespace RestApiService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet(Name = "GetAllOrders")]
        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult GetOrder(int id)
        {
            var order = _orderRepository.Get(id);

            if (order == null)
            {
                return NotFound();
            }

            return new ObjectResult(order);
        }

        [HttpGet("{id}", Name = "GetCacheOrder")]
        public IActionResult GetCacheOrder(int id)
        {
            var order = _orderRepository.GetCache(id);

            if (order == null)
            {
                return NotFound();
            }

            return new ObjectResult(order);
        }

        [HttpPost(Name = "CreateOrder")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            _orderRepository.Create(order);
            return CreatedAtRoute("GetOrder", new { id = order.Id }, order);
        }

        [HttpPut("{id}", Name = "UpdateOrder")]
        public IActionResult UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            if (updatedOrder == null || updatedOrder.Id != id)
            {
                return BadRequest();
            }

            var order = _orderRepository.Get(id);
            if (order == null)
            {
                return NotFound();
            }

            _orderRepository.Update(updatedOrder);
            return CreatedAtRoute("GetOrder", new { id = order.Id }, order);
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        public IActionResult DeleteOrder(int id)
        {
            var deletedOrder = _orderRepository.Delete(id);

            if (deletedOrder == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedOrder);
        }

        [HttpGet(Name = "GetFilteredOrdersWithSorting")]
        public IEnumerable<Order> GetFilteredOrdersWithSorting(int clientId, DateTime dateFrom, DateTime dateTo, string order)
        {
            return _orderRepository.GetFilteredOrdersWithSorting(clientId, dateFrom, dateTo, order);
        }
    }
}
