using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Application.Interfaces
{
    public interface IRepository<TEntity, in TKey>
    {
        public Task<List<TEntity>> GetAllAsync(string includeProperties="");
        public Task<List<TEntity>> GetByIdAsync(Expression<Func<TEntity, bool>> filter);
        public Task<TEntity> FindAsync(TKey id);
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter);
        public Task<bool> DeleteAsync(TKey id);
        public Task<TEntity> InsertAsync(TEntity item);
        public Task<TEntity> UpdateAsync(TEntity item);
        bool Exist(Expression<Func<TEntity, bool>> filter);
    }
}
