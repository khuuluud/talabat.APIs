using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase();
        }

        public async Task CasheResponseasync(string Cachekey, object Response, TimeSpan ExpireTime)
        {
            if (Response is null) return;
            var Options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var SerializedResponse = JsonSerializer.Serialize(Response);
           await _database.StringSetAsync(Cachekey , SerializedResponse , ExpireTime);
        }

        public async Task<string?> GetChachedResponse(string ChacheKey)
        {
          var ChachedResponse = await _database.StringGetAsync(ChacheKey);

            if (ChachedResponse.IsNullOrEmpty) return null;
            return ChachedResponse;

        }
    }
}
