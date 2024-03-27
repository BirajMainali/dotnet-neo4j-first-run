using Neo4j.Driver;

namespace neo4j_first_run;

public class GreetService
{
    private readonly IDriver _driver;

    public GreetService(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<string> GreetingAsync(string message)
    {
        await using var session = _driver.AsyncSession();
        var greeting = await session.ExecuteWriteAsync(
            async tx =>
            {
                var result = await tx.RunAsync(
                    "CREATE (a:Greeting) " +
                    "SET a.message = $message " +
                    "RETURN a.message + ', from node ' + id(a)",
                    new { message });

                var record = await result.SingleAsync();
                return record[0].As<string>();
            });
        return greeting;
    }
}