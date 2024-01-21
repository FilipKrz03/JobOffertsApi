using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Common;
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
        Task<TEntity?> GetByIdAsync(Guid id);
        IQueryable<TEntity> GetByIdQuery(Guid id);
        Task<bool> EntityExistAsync(Guid id);
        void DeleteEntity(TEntity entity);
    }
}
