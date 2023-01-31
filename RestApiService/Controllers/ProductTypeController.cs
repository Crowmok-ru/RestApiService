using Microsoft.AspNetCore.Mvc;
using RestApiService.Context;
using RestApiService.Model;
using System;
using System.Collections.Generic;

namespace RestApiService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProductTypeController : Controller
    {
        public readonly IBasicRepository<ProductType> _basicRepository;

        public ProductTypeController(IBasicRepository<ProductType> basicRepository)
        {
            _basicRepository = basicRepository;
        }

        [HttpGet(Name = "GetAllProductTypes")]
        public IEnumerable<ProductType> GetAllProductTypes()
        {
            return _basicRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetProductType")]
        public IActionResult GetProductType(int id)
        {
            var productType = _basicRepository.Get(id);

            if (productType == null)
            {
                return NotFound();
            }

            return new ObjectResult(productType);
        }

        [HttpGet("{id}", Name = "GetCacheProductType")]
        public IActionResult GetCacheProductType(int id)
        {
            var productType = _basicRepository.GetCache(id);

            if (productType == null)
            {
                return NotFound();
            }

            return new ObjectResult(productType);
        }

        [HttpPost(Name = "AddProductType")]
        public IActionResult CreateProductType([FromBody] ProductType productType)
        {
            if (productType == null)
            {
                return BadRequest();
            }
            _basicRepository.Create(productType);
            return CreatedAtRoute("GetProductType", new { id = productType.Id }, productType);
        }

        [HttpPut("{id}", Name = "UpdateProductType")]
        public IActionResult UpdateProductType(int id, [FromBody] ProductType updatedProductType)
        {
            if (updatedProductType == null || updatedProductType.Id != id)
            {
                return BadRequest();
            }

            var productType = _basicRepository.Get(id);
            if (productType == null)
            {
                return NotFound();
            }

            _basicRepository.Update(updatedProductType);
            return CreatedAtRoute("GetProductType", new { id = productType.Id }, productType);
        }

        [HttpDelete("{id}", Name = "DeleteProductType")]
        public IActionResult DeleteProductType(int id)
        { 
            try
            {
                var deletedProductType = _basicRepository.Delete(id);
                return new ObjectResult(deletedProductType);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
