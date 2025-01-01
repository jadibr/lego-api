using FluentValidation;

namespace lego_api;

public class CreateBrickInputValidator : AbstractValidator<CreateBrickInput>
{
  public CreateBrickInputValidator()
  {
    RuleFor(b => b.inStockCount)
      .GreaterThan(0);
  }
}
