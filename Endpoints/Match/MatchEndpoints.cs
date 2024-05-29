using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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