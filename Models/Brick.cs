namespace lego_api;

public class Brick
{
  public required Guid Id { get; set; }
  public required string PartNumber { get; set; }
  public string? Name { get; set; }
  public BrickColor Color { get; set; }
  public int InStockCount { get; set; }
}
