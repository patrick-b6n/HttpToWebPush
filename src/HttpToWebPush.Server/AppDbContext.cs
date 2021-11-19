using HttpToWebPush.Server.Features.Subscriptions;
using HttpToWebPush.Shared.Features.Subscriptions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace HttpToWebPush.Server;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed class AppDbContext : DbContext
{
    static AppDbContext()
    {
        NpgsqlConnection.GlobalTypeMapper.MapEnum<Channel>();
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<SubscriptionEntity> Subscriptions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresEnum<Channel>();
    }
}