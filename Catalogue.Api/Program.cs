using Catalogue.Api.Config;
using Catalogue.Api.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
// * add health checks with mongodb health check 
builder.Services.AddHealthChecks().AddMongoDb(
    mongoDbSettings.ConnectionString,
    name: "mongodb",
    timeout: TimeSpan.FromSeconds(3),
    tags: new[] { "ready" });

// * register services for injectiion!!!
builder.Services.AddSingleton<IMongoClient>(ServiceProvider =>
{
    // serialize guid & datetime to string in mongodb
    BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
    BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

    return new MongoClient(mongoDbSettings.ConnectionString);
});
builder.Services.AddSingleton<IItemRepository, MongoDbItemsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// * endpoints to check healthy
app.MapHealthChecks("/healthz/ready", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("ready"),
});
app.MapHealthChecks("/healthz/live", new HealthCheckOptions
{
    Predicate = (_) => false
});

// only redirect in dev mode here.. as in container we dont want to redirect (i.e use http not https)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
