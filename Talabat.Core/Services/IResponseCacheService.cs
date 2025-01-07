using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Services
{
    public interface IResponseCacheService
    {
        // cache data
       

        Task CasheResponseasync(string Cachekey ,object Response , TimeSpan ExpireTime );
        // get cached data

        Task<string?> GetChachedResponse(string ChacheKey);

    }
}
