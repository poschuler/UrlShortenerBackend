using System.Reflection;

using DbUp;

string connectionString =
        args.FirstOrDefault()
        ?? "Host=localhost;Port=5433;Database=token-range-generator;Username=postgres;Password=postgres";

EnsureDatabase.For.PostgresqlDatabase(connectionString);

DbUp.Engine.UpgradeEngine upgrader =
    DeployChanges.To
        .PostgresqlDatabase(connectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
        .LogToConsole()
        .Build();

DbUp.Engine.DatabaseUpgradeResult result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(result.Error);
    Console.ResetColor();
#if DEBUG
    Console.ReadLine();
#endif
    return -1;
}

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Success!");
Console.ResetColor();
return 0;
