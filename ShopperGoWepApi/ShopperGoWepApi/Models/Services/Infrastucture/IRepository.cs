// ===============================================================
// File name: IRepository.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using System.Linq.Expressions;

namespace ShopperGoWepApi.Models.Services.Infrastucture
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(TEntity entity);
        void Delete(object id);
        Task DeleteAsync(object id);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");

        TEntity? GetById(object id);
        Task<TEntity?> GetByIdAsync(object id);

        IEnumerable<TEntity> GetWithRawSql(string query,
            params object[] parameters);

        void Insert(TEntity entity);
        Task InsertAsync(TEntity entity);

        void Update(TEntity entity);
    }
}
