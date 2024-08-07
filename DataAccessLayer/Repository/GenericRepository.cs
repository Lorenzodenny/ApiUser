using Microsoft.EntityFrameworkCore;
using UserManagementAPI.DataAccessLayer;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementAPI.Model;
using System.Linq.Expressions;
using UserManagementAPI.Abstract;


namespace UserManagementAPI.DataAccessLayer.Repository
{
 
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TEntity)))
            {
                return await _dbSet.Where(e => !((ISoftDeletable)e).IsDeleted).ToListAsync();
            }
            else
            {
                return await _dbSet.ToListAsync();
            }
        }


        //// Con la reflection
        //public async Task<IEnumerable<TEntity>> GetAllAsync()
        //{
        //    var parameter = Expression.Parameter(typeof(TEntity), "e");
        //    var property = typeof(TEntity).GetProperty("IsDeleted");

        //    if (property != null && property.PropertyType == typeof(bool))
        //    {
        //        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        //        var constant = Expression.Constant(false);
        //        var equality = Expression.Equal(propertyAccess, constant);
        //        var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

        //        return await _dbSet.Where(lambda).ToListAsync();
        //    }
        //    else
        //    {
        //        return await _dbSet.ToListAsync();
        //    }
        //}

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            if (entity != null)
            {
                // Controlla se l'entità ha una proprietà IsDeleted
                if (entity is ISoftDeletable)
                {
                    (entity as ISoftDeletable).IsDeleted = true;
                    _context.Update(entity);
                }
                else
                {
                    _dbSet.Remove(entity);
                }
            }
        }


        //// Con la reflection
        //public void Delete(int id)
        //{
        //    var entity = _context.Set<TEntity>().Find(id);
        //    if (entity != null)
        //    {
        //        var property = typeof(TEntity).GetProperty("IsDeleted");
        //        if (property != null && property.PropertyType == typeof(bool))
        //        {
        //            property.SetValue(entity, true);
        //            _context.Update(entity);
        //        }
        //        else
        //        {
        //            _dbSet.Remove(entity);
        //        }
        //    }
        //}



        public void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}


