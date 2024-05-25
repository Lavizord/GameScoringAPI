using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerTagsSorter : IDocumentFilter
{
    // TODO: This was an attempt to order the Swagger endpoints in a certain way. Doesnt seem to be working.
    private readonly List<string> predefinedOrder = new List<string>
    {
        "Games",
        "Matches",
        "MatchDataPoints",
        "GET Endpoints",
        "POST Endpoints",
        "PUT Endpoints",
        "DELETE Endpoints"       
        // Add more tags as needed
    };

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var tagsMap = new Dictionary<string, OpenApiTag>();

        // Add existing tags to a dictionary
        foreach (var tag in swaggerDoc.Tags)
        {
            tagsMap[tag.Name] = tag;
        }

        // Create a sorted list of tags based on the predefined order
        var sortedTags = predefinedOrder
            .Select(tagName => tagsMap.TryGetValue(tagName, out var tag) ? tag : null)
            .Where(tag => tag != null)
            .ToList();

        // Clear existing tags
        swaggerDoc.Tags.Clear();

        // Add sorted tags individually
        foreach (var tag in sortedTags)
        {
            swaggerDoc.Tags.Add(tag);
        }
    }
}