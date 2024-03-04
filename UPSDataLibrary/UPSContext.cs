using Microsoft.EntityFrameworkCore;

namespace UPSDataLibrary
{
    public class UPSContext(DbContextOptions<UPSContext> options) : DbContext(options)
    {
        public DbSet<UPSData> UPSData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //// Configure the context to retry on failure
            //optionsBuilder.UseMySql(
            //    "server=mysql;database=upsdb;user=root;password=password",
            //    new MySqlServerVersion(new Version(8, 0, 21)),
            //    mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            //        maxRetryCount: 5,        // The number of times to retry before failing
            //        maxRetryDelay: TimeSpan.FromSeconds(10), // The maximum delay between retries
            //        errorNumbersToAdd: null)  // SQL error numbers to consider for retries
            //);
            // If not configured externally (e.g., during testing), then configure here
            if (!optionsBuilder.IsConfigured)
            {
                // Fallback configuration logic or throw an exception
                throw new InvalidOperationException("UPSContext needs to be configured with a connection string.");
            }
        }
    }
}
