﻿using System;
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

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        #endregion

        Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec);

        Task<T> GetByIdWithSpecAsync(ISpecifications<T> spec);

    }
}
