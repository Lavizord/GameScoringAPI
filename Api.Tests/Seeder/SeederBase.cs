using System.Text.Json;

// Implemented following this: https://medium.com/c-sharp-progarmming/seed-test-data-in-dotnet-core-2fe0e363df47

/// <summary>
/// This calss will be extender for each entity or data that needs to be seeded into the database.
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="TData"></typeparam>
public abstract class SeederBase<TDbContext, TData>
where TDbContext : GameDBContext
{

    public TData Data { get; set; }
    public SeederBase()
    {
        InitData();
    }

    private void InitData()
    {
        var seedName = this.GetType().Name;
        var json = File.ReadAllText($"./Data/{seedName}.json");
        this.Data = JsonSerializer.Deserialize<TData>(json);
    }

    public abstract void Execute(TDbContext dbContext);
}