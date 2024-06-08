using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class MapMatchDataPointEndpoints
{
    public static void MapMatchDataPointEndpoints(this WebApplication app)
    {
        app.MapGetMatchDataPointEndpoints();
        app.MapPostMatchDataPointEndpoints();
        app.MapPutMatchDataPointEndpoints();
        app.MapDeleteMatchDataPointEndpoints();
    }
}