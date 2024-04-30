var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Showcase_HealthChecks_ApiService>("apiservice");

builder.AddProject<Projects.Showcase_HealthChecks_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiService);

builder.Build().Run();
