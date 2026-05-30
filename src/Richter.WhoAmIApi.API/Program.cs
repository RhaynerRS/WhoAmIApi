using Richter.WhoAmIApi.IoC;
using Richter.WhoAmIApi.IoC.Config;

var builder = WebApplication.CreateBuilder(args).ConfigureHost();

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.AddControllers().AddCustomJsonOptions();

await services.AddCommonServices(configuration, environment);

var app = builder.Build();

app.UseCommonAppConfiguration();
await app.RunAsync();