namespace lego_api;

public class BrickEntity
{
  public Guid Id { get; set; }

  public string PartNumber { get; set; }

  public string? Name { get; set; }

  public BrickColor Color { get; set; }

  public int InStockCount { get; set; }
  public List<SetEntity> Sets { get; set; }
}