using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repository
{
    public interface iGenericRepository<T> where T : BaseEntity
    {
        #region Without Specifications

        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);

        Task<T> GetEntityWithSpecAsync(ISpecifications<T> spec);


        Task<int> GetCountwithSpecAsync(ISpecifications<T> spec);

        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
      
    }
}
