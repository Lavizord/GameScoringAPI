using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

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