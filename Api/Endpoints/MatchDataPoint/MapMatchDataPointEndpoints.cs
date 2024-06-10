public static class MatchDataPointEndpoints
{
    public static void MapMatchDataPointEndpoints(this WebApplication app)
    {
        app.MapGetMatchDataPointEndpoints();
        app.MapPostMatchDataPointEndpoints();
        app.MapPutMatchDataPointEndpoints();
        app.MapDeleteMatchDataPointEndpoints();
    }
}