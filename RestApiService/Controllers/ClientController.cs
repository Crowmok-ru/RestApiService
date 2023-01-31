using Microsoft.AspNetCore.Mvc;
using RestApiService.Context;
using RestApiService.Model;
using System;
using System.Collections.Generic;

namespace RestApiService.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ClientController : Controller
    {
        private readonly IBasicRepository<Client> _basicRepository;

        public ClientController(IBasicRepository<Client> basicRepository)
        {
            _basicRepository = basicRepository;
        }

        [HttpGet(Name = "GetAllClients")]
        public IEnumerable<Client> GetAllClients()
        {
            return _basicRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetClient")]
        public IActionResult GetClient(int id)
        {
            var client = _basicRepository.Get(id);

            if (client == null)
            {
                return NotFound();
            }

            return new ObjectResult(client);
        }

        [HttpGet("{id}", Name = "GetCacheClient")]
        public IActionResult GetCacheClient(int id)
        {
            var client = _basicRepository.GetCache(id);

            if (client == null)
            {
                return NotFound();
            }

            return new ObjectResult(client);
        }

        [HttpPost(Name = "AddClient")]
        public IActionResult CreateClient([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest();
            }
            _basicRepository.Create(client);
            return CreatedAtRoute("GetClient", new { id = client.Id }, client);
        }

        [HttpPut("{id}" , Name ="UpdateClient")]
        public IActionResult UpdateClient(int id, [FromBody] Client updatedClient)
        {
            if (updatedClient == null || updatedClient.Id != id)
            {
                return BadRequest();
            }

            var client = _basicRepository.Get(id);
            if (client == null)
            {
                return NotFound();
            }

            _basicRepository.Update(updatedClient);
            return CreatedAtRoute("GetClient", new { id = client.Id }, client);
        }

        [HttpDelete("{id}", Name ="DeleteClient")]
        public IActionResult DeleteClient(int id)
        {
            try
            {
                var deletedClient = _basicRepository.Delete(id); 
                return new ObjectResult(deletedClient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
