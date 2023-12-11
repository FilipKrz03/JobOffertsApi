using JobOffersApiCore.BaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task Insert(TEntity entity);
        Task SaveChangesAsync();

        IQueryable<TEntity> Query();

    }
}
