using HttpToWebPush.Server.Features.Subscriptions;
using HttpToWebPush.Shared.Features.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HttpToWebPush.Server;

public sealed class PushCenterDbContext : DbContext
{
    static PushCenterDbContext()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<Channel>("channel");
    }

    public PushCenterDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<SubscriptionEntity> Subscriptions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresEnum<Channel>();
    }
}