using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Application.Interfaces;
using Sprout.Exam.Domain.Entities;

namespace Sprout.Exam.Application.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity
    {
        private readonly DbContext _db;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<TEntity>> GetAllAsync(string includeProperties = "")
        {
            IQueryable<TEntity> res = _db.Set<TEntity>().Where(x => x.IsDeleted == false);
            foreach (var i in includeProperties.Split(","))
            {
                res = res.Include(i);
            }
            return await res.ToListAsync();
        }

        public async Task<List<TEntity>> GetByIdAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _db.Set<TEntity>().Where(filter).ToListAsync();
        }

        public async Task<TEntity> FindAsync(TKey id)
        {
            var qry = _db.Set<TEntity>();
            return await qry.FindAsync(id);
        }
        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            var qry = _db.Set<TEntity>().AsNoTracking();
            return await qry.FirstOrDefaultAsync(filter);
        }

        public async Task<bool> DeleteAsync(TKey id)
        {
            try
            {
                var entity = await FindAsync(id);
                entity.IsDeleted = true;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<TEntity> InsertAsync(TEntity item)
        {
            _db.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task<TEntity> UpdateAsync(TEntity item)
        {
            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return item;
        }

        public bool Exist(Expression<Func<TEntity, bool>> filter)
        {
            return _db.Set<TEntity>().Any(filter);
        }
    }
}
