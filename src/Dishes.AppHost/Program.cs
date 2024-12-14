var builder = DistributedApplication.CreateBuilder(args);

var dishesapi = builder.AddProject<Projects.Dishes_API>("dishesapi");

builder.AddProject<Projects.Dishes_Web>("web")
    .WithReference(dishesapi);

builder.AddProject<Projects.Dishes_Webhook>("dishes-webhook");

builder.Build().Run();
