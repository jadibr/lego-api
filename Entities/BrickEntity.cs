using System.ComponentModel.DataAnnotations;

namespace lego_api;

public class BrickEntity
{
  [Key]
  public Guid Id { get; set; }

  [Required]
  public string PartNumber { get; set; }

  public string? Name { get; set; }

  [Required]
  public BrickColor Color { get; set; }

  [Required]
  public int InStockCount { get; set; }
}