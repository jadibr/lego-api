using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace lego_api;

public class BrickService(IDbContextFactory<AppDbContext> ctxFactory, IMapper mapper)
{
  public async Task<Brick> GetByid(Guid id)
  {
    using var dbContext = await ctxFactory.CreateDbContextAsync();
    BrickEntity? brickEntity = await dbContext.Bricks.FirstOrDefaultAsync(b =>
        b.Id == id) ?? throw new GraphQLException(
        $"Brick {id} doesn't exist");
    return mapper.Map<Brick>(brickEntity);
  }

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
      throw new Exception("Failed to create new brick");
    }
  }

  public async Task<Brick> Update(UpdateBrickInput brickInput)
  {
    try
    {
      using var dbContext = await ctxFactory.CreateDbContextAsync();
      var brickEntity = await dbContext.Bricks.FirstOrDefaultAsync(b =>
        b.Id == brickInput.id) ?? throw new GraphQLException(
        $"Brick {brickInput.id} doesn't exist");

      if (brickInput.partNumber != null && brickEntity.PartNumber != brickInput.partNumber) brickEntity.PartNumber = brickInput.partNumber;
      if (brickInput.name != null && brickEntity.Name != brickInput.name) brickEntity.Name = brickInput.name;
      if (brickInput.color != null && brickEntity.Color != brickInput.color) brickEntity.Color = (BrickColor)brickInput.color;
      if (brickInput.inStockCount != null && brickEntity.InStockCount != brickInput.inStockCount) brickEntity.InStockCount = (int)brickInput.inStockCount;

      await dbContext.SaveChangesAsync();
      return mapper.Map<Brick>(brickEntity);
    }
    catch
    {
      throw new Exception($"Failed to update brick {brickInput.id}");
    }
  }
}
