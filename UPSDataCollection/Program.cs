using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using UPSDataCollection;
using UPSDataLibrary;

var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<UPSContext>(options =>
    options.UseMySql(
        "server=mysql;database=upsdb;user=root;password=password",
        new MySqlServerVersion(new Version(8, 0, 21)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)
    ));

var serviceProvider = serviceCollection.BuildServiceProvider();

using var scope = serviceProvider.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<UPSContext>();
db.Database.EnsureCreated();

var client = new HttpClient();
var timer = new System.Timers.Timer(10000);

timer.Elapsed += async (sender, e) =>
{
    // var response = await client.GetFromJsonAsync<JsonObject>("http://localhost:5003/pwrstat");
    var response = await client.GetFromJsonAsync<JsonObject>("http://pwr_stat:5003/pwrstat");
    if (response != null)
    {
        var data = UPSDataParser.CreateFromJsonObject(response);
        db.Add(data);
        await db.SaveChangesAsync();

        var toDelete = db.UPSData.OrderBy(d => d.Time).TakeWhile((_, i) => db.UPSData.Count() - i > 100).ToList();
        db.UPSData.RemoveRange(toDelete);
        await db.SaveChangesAsync();
    }
};

timer.Start();

// Using ManualResetEvent to block and allow for graceful shutdown
var waitHandle = new ManualResetEvent(false);
Console.CancelKeyPress += (sender, eventArgs) =>
{
    Console.WriteLine("Stopping...");
    eventArgs.Cancel = true;
    timer.Stop();
    timer.Dispose();
    waitHandle.Set();
};

waitHandle.WaitOne();
