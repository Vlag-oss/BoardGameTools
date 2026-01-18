namespace BoardGameTools.FunctionalTests
{
    public interface ITestDatabase
    {
        Task InitializeAsync();
        Task ResetAsync();
        Task DisposeAsync();
    }
}
