using AppAny.HotChocolate.FluentValidation;
using HotChocolate.Authorization;

namespace lego_api;

public class BrickMutation(BrickService brickService)
{
  [Authorize]
  public async Task<Brick> AddBrick(
      [UseFluentValidation] CreateBrickInput brickInput)
  {

    return await brickService.Create(
      new Brick
      {
        Id = Guid.NewGuid(),
        PartNumber = brickInput.partNumber,
        Name = brickInput.name,
        Color = brickInput.color,
        InStockCount = brickInput.inStockCount,
      }
    );
  }
}
