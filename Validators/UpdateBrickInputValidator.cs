using FluentValidation;

namespace lego_api;

public class UpdateBrickInputValidator : AbstractValidator<UpdateBrickInput>
{
  public UpdateBrickInputValidator()
  {
    RuleFor(b => b.inStockCount)
      .GreaterThan(0);
  }
}
