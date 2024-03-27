using Microsoft.Extensions.Options;
using neo4j_first_run;
using Neo4j.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<Neo4JOptions>().Bind(builder.Configuration.GetSection("Neo4J"));

builder.Services.AddSingleton<IDriver>(provider =>
{
    var options = provider.GetRequiredService<IOptions<Neo4JOptions>>().Value;
    return GraphDatabase.Driver(uri: options.Host, AuthTokens.Basic(username: options.Username, password: options.Password));
});

builder.Services.AddScoped<GreetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/Greet", async (string message, GreetService service) => await service.GreetingAsync(message))
    .WithName("Greet")
    .WithOpenApi();

app.Run();