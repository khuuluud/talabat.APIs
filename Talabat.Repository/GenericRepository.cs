using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Core.Specifications;
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

        #region Without specifications
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
                return (IEnumerable<T>)await _dbcontext.Products.Include(P => P.Brand).Include(P => P.Category).ToListAsync();

            else

                return await _dbcontext.Set<T>().ToListAsync();

        }
        public async Task<T> GetByIdAsync(int Id)
            => await _dbcontext.Set<T>().FindAsync(Id);

        #endregion

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbcontext.Set<T>() , spec);
        }
    }
}
