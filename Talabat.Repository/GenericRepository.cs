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
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {

                return await _dbcontext.Set<T>().ToListAsync();

        }
        public async Task<T> GetByIdAsync(int Id)
            => await _dbcontext.Set<T>().FindAsync(Id);

        #endregion

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbcontext.Set<T>() , spec);
        }

        public async Task<int> GetCountwithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task AddAsync(T item)
           => await _dbcontext.Set<T>().AddAsync(item);


        public  void Update(T item)
           => _dbcontext.Set<T>().Update(item);

        public void Delete(T item)
          => _dbcontext.Set<T>().Remove(item);
    }
}
