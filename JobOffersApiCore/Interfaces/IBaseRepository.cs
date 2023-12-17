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
        void Insert(TEntity entity);
        Task SaveChangesAsync();
        void AddRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Query();
        Task<TEntity> GetById(Guid id);
        IQueryable<TEntity> GetByIdQuery(Guid id);
    }
}
