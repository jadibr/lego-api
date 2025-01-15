namespace lego_api;

public class SetBrickEntity
{
  public Guid Id { get; set; }
  public Guid SetId { get; set; }
  public SetEntity Set { get; set; }
  public Guid BrickId { get; set; }
  public BrickEntity Brick { get; set; }
  public int Qty { get; set; }
}