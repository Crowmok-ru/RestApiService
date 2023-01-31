using Microsoft.Extensions.Caching.Memory;
using RestApiService.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestApiService.Context.Repo
{
    public class EFClientRepository : IBasicRepository<Client>
    {
        private readonly EFDbContext _context;  
        private readonly IMemoryCache _cache;

        public EFClientRepository(EFDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public IEnumerable<Client> GetAll()
        {
            return _context.Client;
        }
        public Client Get(int id)
        {
            return _context.Client.Find(id);
        }        

        public Client GetCache(int id)
        {
            _cache.TryGetValue(id, out Client client);
            if (client == null)
            {
                client = _context.Client.Find(id);
                if (client != null)
                {
                    _cache.Set(client.Id, client, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
                    return _context.Client.Find(id);
                }
            }
            return client;

        }

        public void Create(Client client)
        {
            _context.Client.Add(client);
            _context.SaveChanges();
        }
        public void Update(Client updatedClient)
        {
            Client client = Get(updatedClient.Id);
            client.Name = updatedClient.Name;
            client.Phone = updatedClient.Phone;

            _context.Client.Update(client);
            _context.SaveChanges();
        }

        public Client Delete(int id)
        {
            var client = Get(id);
            
            if (client != null)
            {
                var existForeignKey = _context.Order.Where(o=>o.ClientId == id);
                if (existForeignKey != null)
                {
                    var ids = string.Join(",", existForeignKey.Select(s => s.Id).ToList());
                    throw new Exception($"Удаление невозможно т.к. клиент используется в заказах {ids}");
                }
                _context.Client.Remove(client);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Клиент не найден");
            }

            return client;
        }
    }
}
