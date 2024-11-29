using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Repository;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfwork : IUnitOfWork
    {
        private readonly StoredContext _dbContext;
        private Hashtable _repositories;
        public UnitOfwork(StoredContext dbContext)
        {
            _repositories = new Hashtable();
            _dbContext = dbContext;
        }
        public async Task<int> CompleteAsync()
             => await _dbContext.SaveChangesAsync();

        public ValueTask DisposeAsync()
            =>  _dbContext.DisposeAsync();


        public iGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(type, repository);
            }
            return _repositories[type] as iGenericRepository<TEntity>;
        }
    }
}
