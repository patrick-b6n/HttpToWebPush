using HttpToWebPush.Server;
using HttpToWebPush.Server.Common;
using HttpToWebPush.Server.Features.Send;
using HttpToWebPush.Server.Features.Subscriptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// ##################
//      Services
// ##################

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.Configure<PushApiOptions>(builder.Configuration.GetSection("HttpToWebPush:PushApi"));

builder.Services.AddDbContext<AppDbContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("HttpToWebPush"), b => b.MigrationsAssembly("HttpToWebPush.Server"))
);

// custom
builder.Services.AddScoped<PushClient>();

builder.Services.AddScoped<SubscriptionService>();

// ##################
//  Request Pipeline
// ##################

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseBlazorFrameworkFiles();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

// ##################
//      EF Core
// ##################

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    // needed for enum, see https://www.npgsql.org/efcore/mapping/enum.html
    using (var conn = (NpgsqlConnection)context.Database.GetDbConnection())
    {
        conn.Open();
        conn.ReloadTypes();
    }
}

// ##################
//       Start
// ##################

app.Run();