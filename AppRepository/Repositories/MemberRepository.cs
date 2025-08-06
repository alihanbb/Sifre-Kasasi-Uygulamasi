using AppRepository.Context;
using Microsoft.EntityFrameworkCore;

namespace AppRepository.Repositories
{
    public class MemberRepository<T> : IMemberRepository<T> where T : class
    {
        private readonly AuthtakeDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public MemberRepository(AuthtakeDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return dbSet.AsNoTracking();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
