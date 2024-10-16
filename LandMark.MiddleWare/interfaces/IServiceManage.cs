using LandMark.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LandMark.Middleware.interfaces
{
    public interface IServiceManage<TEntity>
    {
        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="ArgumentNullException">instance</exception>
        void Create(TEntity instance);

        void CreateAll(IEnumerable<TEntity> instance);

        void Update(TEntity instance);
        void Delete(TEntity instance);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetAll();
        void SaveChanges();

        IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> predicate) where TResult : class;
    }
}
