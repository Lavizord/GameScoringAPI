public static class MatchEndpoints
{
    public static void MapMatchEndpoints(this WebApplication app)
    {
        app.MapGetMatchEndpoints();
        app.MapPostMatchEndpoints();
        app.MapPutMatchEndpoints();
        app.MapDeleteMatchEndpoints();
    }
}