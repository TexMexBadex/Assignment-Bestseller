using Microsoft.EntityFrameworkCore;
using Products.Data;

namespace Products.Tests;

public class TestDbContextFactory : IDbContextFactory<AppDbContext>
{
  private readonly DbContextOptions<AppDbContext> _options;

  public TestDbContextFactory(DbContextOptions<AppDbContext> options)
  {
    _options = options;
  }

  public AppDbContext CreateDbContext()
  {
    return new AppDbContext(_options);
  }

  public ValueTask<AppDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
  => new(new AppDbContext(_options));
}
