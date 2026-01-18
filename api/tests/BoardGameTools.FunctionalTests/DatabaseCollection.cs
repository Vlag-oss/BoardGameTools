namespace BoardGameTools.FunctionalTests
{
    [CollectionDefinition("Database collection")]
    public sealed class DatabaseCollection : ICollectionFixture<PostgresContainer>
    {

    }
}
