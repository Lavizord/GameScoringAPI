public class GamesSeeder : SeederBase<GameDBContext, List<Game>>
{
    public override void Execute(GameDBContext dbContext)
    {
        Random rnd = new Random();
        // The add will create our Ids, we dont have to generate them or have them
        // in our data
        dbContext.Games.AddRange(Data);
        dbContext.SaveChanges();
    }
}