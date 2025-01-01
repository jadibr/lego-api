using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace lego_api;

public class BrickService(IDbContextFactory<AppDbContext> ctxFactory, IMapper mapper)
{
  public async Task<IEnumerable<Brick>> GetAll()
  {
    try
    {
      using var dbContext = await ctxFactory.CreateDbContextAsync();
      var brickEntities = await dbContext.Bricks.ToListAsync();
      return mapper.Map<IEnumerable<Brick>>(brickEntities);
    }
    catch
    {
      throw new Exception("Failed to retrieve all bricks");
    }
  }

  public async Task<Brick> Create(Brick brick)
  {
    try
    {
      using var dbContext = await ctxFactory.CreateDbContextAsync();
      var brickEntity = mapper.Map<BrickEntity>(brick);
      dbContext.Bricks.Add(brickEntity);
      await dbContext.SaveChangesAsync();
      return mapper.Map<Brick>(brickEntity);
    }
    catch
    {
      throw new Exception($"Failed to create new brick");
    }
  }
}
