using Microsoft.AspNetCore.Mvc;
using RestApiService.Context;
using RestApiService.Model;
using System;
using System.Collections.Generic;

namespace RestApiService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OrderPositionsController : Controller
    {
        private readonly IOrderPositionsRepository _orderPositions;

        public OrderPositionsController(IOrderPositionsRepository orderPositions)
        {
            _orderPositions = orderPositions;
        }

        [HttpGet(Name = "GetAll")]
        public IEnumerable<OrderPositions> GetAll()
        {
            return _orderPositions.GetAll();
        }

        [HttpGet("{id}", Name = "GetOrderPosition")]
        public IActionResult GetOrderPosition(int id)
        {
            var result = _orderPositions.Get(id);

            if (result == null)
            {
                return NotFound();
            }

            return new ObjectResult(result);
        }

        [HttpGet("{id}", Name = "GetCacheOrderPosition")]
        public IActionResult GetCacheOrderPosition(int id)
        {
            var orderPosition = _orderPositions.GetCache(id);

            if (orderPosition == null)
            {
                return NotFound();
            }

            return new ObjectResult(orderPosition);
        }

        [HttpPost(Name = "AddOrder")]
        public IActionResult CreateOrder([FromBody] OrderPositions orderPositions, int clientId)
        {
            if (orderPositions == null)
            {
                return BadRequest();
            }

            try
            {
                _orderPositions.CreateOrder(clientId, orderPositions);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

            return CreatedAtRoute("GetOrderPosition", new { id = orderPositions.Id }, orderPositions);
        }

        [HttpDelete("{id}", Name = "DeleteOrderPosition")]
        public IActionResult DeleteOrderPosition(int id)
        {
            var deletedOrderPosition = _orderPositions.Delete(id);

            if (deletedOrderPosition == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedOrderPosition);
        }
    }
}
