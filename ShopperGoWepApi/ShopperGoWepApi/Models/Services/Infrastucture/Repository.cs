// ===============================================================
// File name: Repository.cs
// Copyright (c) 2022 - ShopperGoWepApi - Ivan Vanogi
// Creation date: 2022.11.28
// ===============================================================

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ShopperGoWepApi.Models.Services.Infrastucture
{
    /// <summary>
    /// La classe <c>Repository</c> modella un'accesso ai dati generico.
    /// </summary>
    /// <typeparam name="TEntity">Tipo di entita su cui si lavora</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal ApplicationDBContext context;
        internal DbSet<TEntity> dbSet;

        private IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>>? 
            filter, string includeProperties)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    // Oggetti da includere nell'interrogazione
                    query = query.Include(includeProperty);
                }
            }

            return query;
        }



        /// <summary>
        /// Estrazione dei dati con un'interrogazione a sql basso livello
        /// </summary>
        /// <param name="query">stringa SQL dell'interrogazione</param>
        /// <param name="parameters">elenco dei paramentri</param>
        /// <returns>Elenco delle entità che soddisfano l'interrogazione</returns>
        public virtual IEnumerable<TEntity> GetWithRawSql(string query,
            params object[] parameters)
        {
            return dbSet.FromSqlRaw(query, parameters).ToList();
        }

        /// <summary>
        /// Estrazione dei dati con un'interrogazione LINQ
        /// </summary>
        /// <param name="filter">filtro applicabile</param>
        /// <param name="orderBy">ordinamento</param>
        /// <param name="includeProperties">eventuali proprietà da includere</param>
        /// <returns>Elenco di entità che soddisfano l'interrogazione</returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = GetQuery(filter, includeProperties);

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }

        /// <summary>
        /// Estrazione dei dati con un'interrogazione LINQ
        /// </summary>
        /// <param name="filter">filtro applicabile</param>
        /// <param name="orderBy">ordinamento</param>
        /// <param name="includeProperties">eventuali proprietà da includere</param>
        /// <returns>Elenco di entità che soddisfano l'interrogazione</returns>
        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, 
            IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = GetQuery(filter, includeProperties);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            else
                return await query.ToListAsync();
        }

        // --

        /// <summary>
        /// Estrazione dei dati tramite codice identificativo
        /// </summary>
        /// <param name="id">identificativo dell'entità</param>
        /// <returns>Entità che è refenziata all'identificativo ricercato</returns>
        public virtual TEntity? GetById(object id)
        {
            return dbSet.Find(id);
        }
        /// <summary>
        /// Estrazione dei dati tramite codice identificativo
        /// </summary>
        /// <param name="id">identificativo dell'entità</param>
        /// <returns>Entità che è refenziata all'identificativo ricercato</returns>
        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        /// <summary>
        /// Inserimento di una nuova entità
        /// </summary>
        /// <param name="entity">Entità da inserire.</param>
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        /// <summary>
        /// Inserimento di una nuova entità
        /// </summary>
        /// <param name="entity">Entità da inserire.</param>
        public virtual async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }
        
        // --

        /// <summary>
        /// Cancella dalla banca dati l'identificativo indicato
        /// </summary>
        /// <param name="id">Cancellazione di una entità con l'identificativo indicato</param>
        public virtual void Delete(object id)
        {
            TEntity? e = dbSet.Find(id);

            if (e != null)
                Delete(e);
        }
        /// <summary>
        /// Cancella dalla banca dati l'identificativo indicato
        /// </summary>
        /// <param name="id">Cancellazione di una entità con l'identificativo indicato</param>
        public async virtual Task DeleteAsync(object id)
        {
            TEntity? e = await dbSet.FindAsync(id);

            if (e != null)
                Delete(e);
        }
        /// <summary>
        /// Cancellazione di un'entità
        /// </summary>
        /// <param name="entity">Entità da cancellare</param>
        public virtual void Delete(TEntity entity)
        {
            // Se non è fra gli oggi in sessione viene collegato
            if (context.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            // Cancella l'oggetto
            dbSet.Remove(entity);
        }
        
        // --

        /// <summary>
        /// Aggiorna l'entità
        /// </summary>
        /// <param name="entity">Entità da aggiornare</param>
        public virtual void Update(TEntity entity)
        {
            dbSet.Attach(entity);

            // Marca come modificato
            context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Persistenza dei dati modificati
        /// </summary>
        public void Save()
        {
            context.SaveChanges();
        }

        /// <summary>
        /// Persistenza dei dati modificati
        /// </summary>
        public void SaveAsync()
        {
            context.SaveChangesAsync();
        }

        /// <summary>
        /// Questo costruttore inizializza il <c>Repository</c> per l'accesso alla banca dati
        /// (<paramref name="name"/>,<paramref name="country"/>).
        /// </summary>
        /// <param name="dbContext">Connessione alla banca dati</param>
        public Repository(ApplicationDBContext dbContext)
        {
            this.context = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }
    }
}
