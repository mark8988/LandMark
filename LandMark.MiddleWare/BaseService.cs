using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Transactions;
using LandMark.EF;
using LandMark.Middleware.interfaces;

namespace LandMark.Middleware
{
    public class BaseService<TEntity> : IServiceManage<TEntity> where TEntity : class
    {
        private GlobalSettings _globalSettings;
        private LandMarkContext _db;
        private IHttpContextAccessor _httpContextAccessor;



        public BaseService(GlobalSettings globalSettings
            , LandMarkContext db,
IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="ArgumentNullException">instance</exception>
        public virtual void Create(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                var UserAcc = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(o => o.Type.Equals("UserAcc"))?.Value;

                if (instance.GetType().GetProperty("CreatedTime") != null)
                {
                    instance.GetType().GetProperty("CreatedTime").SetValue(instance, DateTime.Now);
                }
                if (instance.GetType().GetProperty("Creator") != null)
                {
                    instance.GetType().GetProperty("Creator").SetValue(instance, UserAcc);
                }

                if (instance.GetType().GetProperty("ModifiedTime") != null)
                {
                    instance.GetType().GetProperty("ModifiedTime").SetValue(instance, DateTime.Now);
                }

                if (instance.GetType().GetProperty("Modifier") != null)
                {
                    instance.GetType().GetProperty("Modifier").SetValue(instance, UserAcc);
                }

                _db.Set<TEntity>().Add(instance);
                SaveChanges();
            }
        }

        public virtual void CreateAll(IEnumerable<TEntity> instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                var UserAcc = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(o => o.Type.Equals("UserAcc"))?.Value;

                foreach (var item in instance)
                {
                    //UpdateCommomInfo(ref item, EntityState.Added);

                    if (item.GetType().GetProperty("CreatedTime") != null)
                    {
                        item.GetType().GetProperty("CreatedTime").SetValue(item, DateTime.Now);
                    }
                    if (instance.GetType().GetProperty("Creator") != null)
                    {
                        instance.GetType().GetProperty("Creator").SetValue(instance, UserAcc);
                    }

                    if (item.GetType().GetProperty("ModifiedTime") != null)
                    {
                        item.GetType().GetProperty("ModifiedTime").SetValue(item, DateTime.Now);
                    }

                    if (instance.GetType().GetProperty("Modifier") != null)
                    {
                        instance.GetType().GetProperty("Modifier").SetValue(instance, UserAcc);
                    }
                    _db.Set<TEntity>().Add(item);
                }

                SaveChanges();
            }
        }


        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Update(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                var UserAcc = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(o => o.Type.Equals("UserAcc"))?.Value;

                if (instance.GetType().GetProperty("Modifier") != null)
                {
                    instance.GetType().GetProperty("Modifier").SetValue(instance, UserAcc);
                }
                if (instance.GetType().GetProperty("ModifiedTime") != null)
                {
                    instance.GetType().GetProperty("ModifiedTime").SetValue(instance, DateTime.Now);
                }

                _db.Entry(instance).State = EntityState.Modified;
                SaveChanges();
            }
        }

        public virtual void UpdateAll(IEnumerable<TEntity> instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                var UserAcc = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(o => o.Type.Equals("UserAcc"))?.Value;

                foreach (var item in instance)
                {
                    if (instance.GetType().GetProperty("Modifier") != null)
                    {
                        instance.GetType().GetProperty("Modifier").SetValue(instance, UserAcc);
                    }
                    if (item.GetType().GetProperty("ModifiedTime") != null)
                    {
                        item.GetType().GetProperty("ModifiedTime").SetValue(item, DateTime.Now);
                    }

                    _db.Entry(item).State = EntityState.Modified;
                }
                SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Delete(TEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                _db.Entry(instance).State = EntityState.Deleted;
                SaveChanges();
            }
        }

        /// <summary>
        /// Deletes all the specified instance.
        /// </summary>
        /// <param name="instance"></param>
        public virtual void DeleteAll(IEnumerable<TEntity> instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            else
            {
                for (int i = 0; i < instance.Count(); i++)
                {
                    _db.Entry(instance.ElementAt(i)).State = EntityState.Deleted;
                }

                //foreach (var item in instance)
                //{
                //    _db.Entry(item).State = EntityState.Deleted;
                //}

                SaveChanges();
            }
        }

        /// <summary>
        /// Gets the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress,
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadUncommitted
                    }))
            {
                return _db.Set<TEntity>().FirstOrDefault(predicate);
            }
        }

        public List<TEntity> GetAll()
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress,
                   new TransactionOptions
                   {
                       IsolationLevel = IsolationLevel.ReadUncommitted
                   }))
            {
                return _db.Set<TEntity>().AsQueryable().ToList();
            }
        }

        public class EntitysByPage
        {
            public List<TEntity> Entities { get; set; }
            public int CurrentPage { get; set; }
            public int CurrentStartPos { get; set; }
            public int CurrentEndPos { get; set; }
            public int TotalPages { get; set; }
            public int TotalItems { get; set; }
        }
        public EntitysByPage GetAllByPage(int currentPage = 1, int rowsPerPage = 10, List<KeyValuePair<string, bool>> order = null)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress,
                   new TransactionOptions
                   {
                       IsolationLevel = IsolationLevel.ReadUncommitted
                   }))
            {
                // 獲取所有資料
                List<TEntity> dbData = _db.Set<TEntity>().ToList();

                // 排序
                if (order != null && order.Count > 0)
                {
                    IOrderedQueryable<TEntity> orderedQuery = null;

                    foreach (var item in order)
                    {
                        var propertyInfo = typeof(TEntity).GetProperty(item.Key);

                        if (propertyInfo != null)
                        {
                            if (orderedQuery == null)
                            {
                                orderedQuery = item.Value
                                    ? dbData.AsQueryable().OrderByDescending(e => propertyInfo.GetValue(e, null))
                                    : dbData.AsQueryable().OrderBy(e => propertyInfo.GetValue(e, null));
                            }
                            else
                            {
                                orderedQuery = item.Value
                                    ? orderedQuery.ThenByDescending(e => propertyInfo.GetValue(e, null))
                                    : orderedQuery.ThenBy(e => propertyInfo.GetValue(e, null));
                            }
                        }
                    }

                    if (orderedQuery != null)
                    {
                        dbData = orderedQuery.ToList();
                    }
                }

                // 獲取所有記錄的總數
                int totalRecords = dbData.Count();

                // 計算總頁數
                int totalPages = (int)Math.Ceiling((double)totalRecords / rowsPerPage);

                // 獲取當前頁的記錄
                List<TEntity> entities = dbData
                                                 .Skip((currentPage - 1) * rowsPerPage)
                                                 .Take(rowsPerPage)
                                                 .ToList();
                return new EntitysByPage
                {
                    Entities = entities,
                    CurrentPage = currentPage,
                    CurrentStartPos = (currentPage - 1) * rowsPerPage + 1,
                    CurrentEndPos = Math.Min(currentPage * rowsPerPage, totalRecords),
                    TotalPages = totalPages,
                    TotalItems = dbData.Count()
                };
            }
        }

        /// <summary>
        /// Gets the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return _db.Set<TEntity>().Where(predicate).ToList();
        }

        public virtual EntitysByPage GetListByPage(Expression<Func<TEntity, bool>> predicate, int currentPage = 1, int rowsPerPage = 10, List<KeyValuePair<string, bool>> order = null)
        {
            // 獲取資料
            List<TEntity> dbData = _db.Set<TEntity>().Where(predicate).ToList();

            // 排序
            if (order != null && order.Count > 0)
            {
                IOrderedQueryable<TEntity> orderedQuery = null;

                foreach (var item in order)
                {
                    var propertyInfo = typeof(TEntity).GetProperty(item.Key);

                    if (propertyInfo != null)
                    {
                        if (orderedQuery == null)
                        {
                            orderedQuery = item.Value
                                ? dbData.AsQueryable().OrderByDescending(e => propertyInfo.GetValue(e, null))
                                : dbData.AsQueryable().OrderBy(e => propertyInfo.GetValue(e, null));
                        }
                        else
                        {
                            orderedQuery = item.Value
                                ? orderedQuery.ThenByDescending(e => propertyInfo.GetValue(e, null))
                                : orderedQuery.ThenBy(e => propertyInfo.GetValue(e, null));
                        }
                    }
                }

                if (orderedQuery != null)
                {
                    dbData = orderedQuery.ToList();
                }
            }

            // 獲取所有記錄的總數
            int totalRecords = dbData.Count();

            // 計算總頁數
            int totalPages = (int)Math.Ceiling((double)totalRecords / rowsPerPage);

            // 獲取當前頁的記錄
            List<TEntity> entities = dbData
                                             .Skip((currentPage - 1) * rowsPerPage)
                                             .Take(rowsPerPage)
                                             .ToList();

            return new EntitysByPage
            {
                Entities = entities,
                CurrentPage = currentPage,
                CurrentStartPos = (currentPage - 1) * rowsPerPage + 1,
                CurrentEndPos = Math.Min(currentPage * rowsPerPage, totalRecords),
                TotalPages = totalPages,
                TotalItems = dbData.Count()
            };

        }

        /// <summary>
        /// Gets the specified predicate counts.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Suppress,
                    new TransactionOptions
                    {
                        IsolationLevel = IsolationLevel.ReadUncommitted
                    }))
            {
                return _db.Set<TEntity>().Where(predicate).Count();
            }
        }

        public bool Check(Expression<Func<TEntity, bool>> express)
        {
            return _db.Set<TEntity>().Any(express);
        }


        public IQueryable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> predicate) where TResult : class
        {
            return _db.Set<TEntity>().Select(predicate);
        }


        public void SaveChanges()
        {
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                SqlException se = null;
                Exception next = ex;

                while (next.InnerException != null)
                {
                    se = next.InnerException as SqlException;
                    next = next.InnerException;
                }

                if (se != null)
                {
                    if (se.Number == 2627)
                        throw new Exception("不可新增重複的資料", se);
                    if (se.Number == 2601)
                        throw new Exception("已經有重複的資料，無法新增", se);
                    if (se.Number == 547)
                        throw new Exception("尚有明細資料，無法刪除", se);
                    else
                        throw se;
                }
                else
                {
                    throw ex;
                }

            }
        }
    }
}
