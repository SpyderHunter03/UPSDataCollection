using Microsoft.EntityFrameworkCore;
using UPSDataLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<UPSContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/upsdata", async (UPSContext context) =>
{
    try
    {
        var data = await context.UPSData.ToListAsync();
        return Results.Ok(data);
    }
    catch (Exception ex)
    {
        // Log the exception details for debugging purposes
        Console.WriteLine($"An error occurred: {ex.Message}");
        return Results.Problem("An error occurred while retrieving UPS data.");
    }
});

app.Run();
