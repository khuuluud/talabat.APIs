using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : iGenericRepository<T> where T : BaseEntity

    {
        private readonly StoredContext _dbcontext;

        public GenericRepository(StoredContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
                return (IEnumerable<T>)await _dbcontext.Products.Include(P => P.Brand).Include(P => P.Category).ToListAsync();

            else

          return await _dbcontext.Set<T>().ToListAsync();

        }
        
        public async Task<T> GetByIdAsync(int Id)
            => await _dbcontext.Set<T>().FindAsync(Id);

    }
}
