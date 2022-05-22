using Microsoft.OpenApi.Models;
using Marketplace.Api;
using Marketplace.Domain;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

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
        FindIdentityProperty = m => m.Name == "_databaseId"
    }
};
store.Conventions.RegisterAsyncIdConvention<ClassifiedAd>(
    (dbName, entity) => Task.FromResult("ClassifiedAd/" + entity.Id.ToString()));
store.Initialize();

builder.Services.AddTransient(c => store.OpenAsyncSession());
builder.Services.AddScoped<ICurrencyLookup, Marketplace.FixedCurrencyLookup>();
builder.Services.AddScoped<IClassifiedAdRepository, Marketplace.ClassifiedAdRepository>();
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