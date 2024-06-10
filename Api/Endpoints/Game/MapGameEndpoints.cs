public static class GameEndpoints
{
    public static void MapGameEndpoints(this WebApplication app)
    {
       app.MapGetGameEndpoints();
       app.MapPostGameEndpoints();
       app.MapPutGameEndpoints();
       app.MapDeleteGameEndpoints();
    }
}