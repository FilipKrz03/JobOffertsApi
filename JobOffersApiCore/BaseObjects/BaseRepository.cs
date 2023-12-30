using JobOffersApiCore.Common;
using JobOffersApiCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.BaseObjects
{
    public abstract class BaseRepository<TDbContext, TEntity> : IBaseRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : BaseEntity
    {

        private readonly TDbContext _context;

        protected BaseRepository(TDbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Query()
        {
            return _context.Set<TEntity>();
        }

        public void Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.AddRange(entities);
        }

        public async Task<TEntity> GetById(Guid id)
        {
            return await _context.Set<TEntity>().Where(e => e.Id == id).SingleAsync();
        }

        public IQueryable<TEntity> GetByIdQuery(Guid id)
        {
            return _context.Set<TEntity>().Where(e => e.Id == id);
        }

        public async Task<bool> EntityExistAsync(Guid id)
        {
            return await _context.Set<TEntity>()
                .Where(e => e.Id == id)
                .AnyAsync();
        }

        public void DeleteEntity(TEntity entity)
        {
            _context.Set<TEntity>()
                .Remove(entity);
        }
    }
}
