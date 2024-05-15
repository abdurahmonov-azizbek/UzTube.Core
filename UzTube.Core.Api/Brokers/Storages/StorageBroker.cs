
using Microsoft.EntityFrameworkCore;
using STX.EFxceptions.SqlServer;
using UzTube.Core.Api.Models.VideoMetadatas;

namespace UzTube.Core.Api.Brokers.Storages;

public partial class StorageBroker : EFxceptionsContext, IStorageBroker
{
    private readonly IConfiguration _configuration;

    public StorageBroker(IConfiguration configuration)
    {
        _configuration = configuration;
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.UseSqlServer(connectionString);
    }

    private async ValueTask<T> InsertAsync<T>(T @object)
    {
        this.Entry(@object).State = EntityState.Added;
        await this.SaveChangesAsync();

        return @object;
    }

    private IQueryable<T> SelectAll<T>() where T : class => this.Set<T>().AsQueryable();

    private async ValueTask<T> SelectAsync<T>(params object[] @objectIds) where T : class =>
         await this.FindAsync<T>(objectIds);


    private async ValueTask<T> UpdateAsync<T>(T @object)
    {
        this.Entry(@object).State = EntityState.Modified;
        await this.SaveChangesAsync();

        return @object;
    }

    private async ValueTask<T> DeleteAsync<T>(T @object)
    {
        this.Entry(@object).State = EntityState.Deleted;
        await this.SaveChangesAsync();

        return @object;
    }
}
