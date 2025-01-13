using HotChocolate.Authorization;

namespace lego_api;

public class BrickQuery(BrickService brickService)

{
  [Authorize]
  public async Task<IEnumerable<Brick>> GetBricks() => await brickService.GetAll();
  [Authorize]
  public async Task<Brick> GetBrick(Guid id) => await brickService.GetByid(id);
}
