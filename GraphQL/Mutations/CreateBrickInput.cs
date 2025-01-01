namespace lego_api;

public record CreateBrickInput(
    string partNumber,
    string? name,
    BrickColor color,
    int inStockCount);
