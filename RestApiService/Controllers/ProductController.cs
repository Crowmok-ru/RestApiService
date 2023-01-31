using Microsoft.AspNetCore.Mvc;
using RestApiService.Context;
using RestApiService.Model;
using System.Collections.Generic;

namespace RestApiService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet(Name = "GetAllProducts")]
        public IEnumerable<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var product = _productRepository.Get(id);

            if (product == null)
            {
                return NotFound();
            }

            return new ObjectResult(product);
        }

        [HttpGet("{id}", Name = "GetCacheProduct")]
        public IActionResult GetCacheProduct(int id)
        {
            var product = _productRepository.GetCache(id);

            if (product == null)
            {
                return NotFound();
            }

            return new ObjectResult(product);
        }

        [HttpPost(Name = "CreateProduct")]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            _productRepository.Create(product);
            return CreatedAtRoute("GetProductType", new { id = product.Id }, product);
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (updatedProduct == null || updatedProduct.Id != id)
            {
                return BadRequest();
            }

            var product = _productRepository.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Update(updatedProduct);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpDelete("{id}", Name = "DeleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            var deletedProduct = _productRepository.Delete(id);

            if (deletedProduct == null)
            {
                return BadRequest();
            }

            return new ObjectResult(deletedProduct);
        }


        [HttpGet(Name = "GetFilteredProductsWithSorting")]
        public IEnumerable<Product> GetFilteredProductsWithSorting(int productTypeId, bool isStockAvailability, string order)
        {
            return _productRepository.GetFilteredProductsWithSorting(productTypeId, isStockAvailability, order);
        }        
    }
}
