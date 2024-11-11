using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public class SpecificationEvalutor<T> where T : BaseEntity
    {
        //Fun To Build Query

        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery , ISpecifications<T> spec)
        {
            var Query = inputQuery;
            if (spec.Criteria is not null)
                Query = Query.Where(spec.Criteria);

            if (spec.OrderBy is not null)
                Query = Query.OrderBy(spec.OrderBy);
            
            if (spec.OrderByDesc is not null) 
                Query = Query.OrderByDescending(spec.OrderByDesc);

            Query = spec.Includes.Aggregate(Query, (CurrentQuery , IncludeExpression) => CurrentQuery.Include(IncludeExpression));
            return Query;
        }
    }
}
