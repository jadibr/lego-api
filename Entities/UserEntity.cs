using System.ComponentModel.DataAnnotations;

namespace lego_api;

public class UserEntity
{
  [Key]
  public Guid Id { get; set; }

  [Required]
  public string Email { get; set; }

  [Required]
  public string Password { get; set; }

}