using Dishes.Common.Configurations;
using Dishes.Common.Extensions;
using Dishes.Webhook.Data;
using Dishes.Webhook.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDatabase<WebhookDbContext>(builder.Configuration
      .GetRequiredSection("Database").Get<DatabaseConfiguration>());

builder.Services.AddScoped<IWebhookRepository, WebhookRepository>();

builder.Services.AddSwaggerGenWithExtraSetup(builder.Configuration, "Controllers");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
