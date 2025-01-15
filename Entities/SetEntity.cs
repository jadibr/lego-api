namespace lego_api;

public class SetEntity
{
  public Guid Id { get; set; }

  public string SetNumber { get; set; }
  public string? Name { get; set; }

  public int InStockCount { get; set; }
   public List<BrickEntity> Bricks { get; set; }
}