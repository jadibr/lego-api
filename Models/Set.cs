namespace lego_api;

public class Set
{
  public required Guid Id { get; set; }
  public required string SetNumber { get; set; }
  public string? Name { get; set; }
  public List<SetBrick> Bricks { get; set; }
  public int InStockCount { get; set; }
}