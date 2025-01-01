namespace lego_api;

public class Query(BrickService brickService)
{
  public BrickQuery Brick { get; } = new BrickQuery(brickService);
}