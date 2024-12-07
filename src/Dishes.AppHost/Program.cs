var builder = DistributedApplication.CreateBuilder(args);

var dishesapi = builder.AddProject<Projects.Dishes_API>("dishesapi");

builder.AddProject<Projects.Dishes_Web>("web")
    .WithReference(dishesapi);

builder.Build().Run();
