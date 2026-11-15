using Microsoft.EntityFrameworkCore;
using Products.Data;

namespace Products.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
  private readonly IDbContextFactory<AppDbContext> _contextFactory;

  public Repository(IDbContextFactory<AppDbContext> contextFactory)
  {
    _contextFactory = contextFactory;
  }

  public async Task<T?> GetByIdAsync(int id)
  {
    using var context = await _contextFactory.CreateDbContextAsync();
    return await context.Set<T>().FindAsync(id);
  }

  public async Task<IEnumerable<T>> GetAllAsync()
  {
    using var context = await _contextFactory.CreateDbContextAsync();
    return await context.Set<T>().ToListAsync();
  }

  public async Task AddAsync(T entity)
  {
    using var context = await _contextFactory.CreateDbContextAsync();
    await context.Set<T>().AddAsync(entity);
    await context.SaveChangesAsync();
  }

  public async Task UpdateAsync(T entity)
  {
    using var context = await _contextFactory.CreateDbContextAsync();
    context.Set<T>().Update(entity);
    await context.SaveChangesAsync();
  }

  public async Task DeleteAsync(T entity)
  {
    using var context = await _contextFactory.CreateDbContextAsync();
    context.Set<T>().Remove(entity);
    await context.SaveChangesAsync();
  }

  public async Task SaveChangesAsync()
  {
    using var context = await _contextFactory.CreateDbContextAsync();
    await context.SaveChangesAsync();
  }
}
