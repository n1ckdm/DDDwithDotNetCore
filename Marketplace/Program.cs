using Microsoft.OpenApi.Models;
using Marketplace.Api;
using Marketplace.Domain;
using Marketplace.Framework;
using Marketplace.Infrastructure;
using Raven.Client.Documents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts => 
{
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ClassifiedAds"
    });
});

var store = new DocumentStore
{
    Urls = new[] {"http://localhost:8080"},
    Database = "Marketplace",
    Conventions =
    {
        FindIdentityProperty = m => m.Name == "DbId"
    }
};
store.Conventions.RegisterAsyncIdConvention<ClassifiedAd>(
    (dbName, entity) => Task.FromResult("ClassifiedAd/" + entity.Id.ToString()));
store.Initialize();

builder.Services.AddScoped<ICurrencyLookup, Marketplace.FixedCurrencyLookup>();
builder.Services.AddScoped(c => store.OpenAsyncSession());
builder.Services.AddScoped<IUnitOfWork, RavenDbUnitOfWork>();
builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
builder.Services.AddScoped<ClassifiedAdsApplicationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.MapControllers();
// app.UseAuthorization();

app.Run();