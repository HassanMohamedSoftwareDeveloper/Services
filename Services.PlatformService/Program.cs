using Microsoft.EntityFrameworkCore;
using Services.PlatformService.AsyncDataServices;
using Services.PlatformService.Data;
using Services.PlatformService.SyncDataServices.Grpc;
using Services.PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConn")));
}
else
{
    Console.WriteLine("--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(options
        => options.UseInMemoryDatabase("InMem"));
}
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddGrpc();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine($"--> Command Service URL {builder.Configuration["CommandService"]}");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/protos/platforms.proto", async context =>
 {
     await context.Response.WriteAsync(File.ReadAllText("Protos/Platforms.proto"));
 });

PrepDb.PrepPopulation(app, app.Environment.IsProduction());
app.Run();
