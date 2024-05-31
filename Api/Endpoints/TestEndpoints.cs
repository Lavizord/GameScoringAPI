
public static class TestEndpoints
{
    public static void MapTestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/testok", async () =>
        {
            return Results.Ok("This is a test OK endpoint, no database acessed.");
        })
        .WithName("GetTestOk")
        .WithTags("Test", "GET Endpoints")
        .WithOpenApi();
    }
}