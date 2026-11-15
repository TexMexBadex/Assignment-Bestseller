using Microsoft.EntityFrameworkCore;
using Products.Data;

namespace Products.Repositories;

public class Repository<T>(IDbContextFactory<AppDbContext> contextFactory) : IRepository<T>
  where T : class
{
  public async Task<T?> GetByIdAsync(int id)
  {
    await using var context = await contextFactory.CreateDbContextAsync();
    return await context.Set<T>().FindAsync(id);
  }

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    await using var context = await contextFactory.CreateDbContextAsync();
    return await context.Set<T>().AsNoTracking().ToListAsync();
  }

  public async Task AddAsync(T entity)
  {
    await using var context = await contextFactory.CreateDbContextAsync();
    context.Set<T>().Add(entity);
    await context.SaveChangesAsync();
  }

  public async Task UpdateAsync(T entity)
  {
    await using var context = await contextFactory.CreateDbContextAsync();
    context.Set<T>().Update(entity);
    await context.SaveChangesAsync();
  }

  public async Task DeleteAsync(T entity)
  {
    await using var context = await contextFactory.CreateDbContextAsync();
    context.Set<T>().Remove(entity);
    await context.SaveChangesAsync();
  }
}