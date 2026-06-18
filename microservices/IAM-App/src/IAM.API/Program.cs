using IAM.BuildingBlocks.Eventing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEventDispatcher, InMemoryEventDispatcher>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => Results.Ok("IAM API - Modular Monolith"));
app.MapControllers();

app.Run();
