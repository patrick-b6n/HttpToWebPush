using HttpToWebPush.Server;
using HttpToWebPush.Server.Common;
using HttpToWebPush.Server.Features.Send;
using HttpToWebPush.Server.Features.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.Configure<PushApiOptions>(builder.Configuration.GetSection("HttpToWebPush:PushApi"));

builder.Services.AddDbContext<PushCenterDbContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("HttpToWebPush"), b => b.MigrationsAssembly("HttpToWebPush.Server"))
);

// custom
builder.Services.AddScoped<PushClient>();

builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<SubscriptionTypeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseBlazorFrameworkFiles();
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PushCenterDbContext>();
    context.Database.Migrate();

    // needed for enum, see https://www.npgsql.org/efcore/mapping/enum.html
    using (var conn = (NpgsqlConnection)context.Database.GetDbConnection())
    {
        conn.Open();
        conn.ReloadTypes();
    }
}

app.Run();