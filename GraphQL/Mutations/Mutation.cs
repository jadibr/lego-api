namespace lego_api;

public class Mutation(BrickService brickService, AuthService authService)
{
    [GraphQLType(typeof(BrickMutation))]
    public BrickMutation Brick { get; } = new BrickMutation(brickService);

    [GraphQLType(typeof(AuthMutation))]
    public AuthMutation Auth { get; } = new AuthMutation(authService);
}