using HotChocolate.Authorization;

namespace lego_api;

public class BrickQuery(BrickService brickService)
{
  [Authorize]
  public async Task<IEnumerable<Brick>> GetBricks() => await brickService.GetAll();
}
