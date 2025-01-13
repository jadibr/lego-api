namespace lego_api;

public record UpdateBrickInput(
    Guid id,
    string? partNumber,
    string? name,
    BrickColor? color,
    int? inStockCount);
