using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Richter.WhoAmIApi.IoC;
using Richter.WhoAmIApi.IoC.Config;

var builder = WebApplication.CreateBuilder(args).ConfigureHost();

var requireAuthPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(requireAuthPolicy);
var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.AddControllers().AddCustomJsonOptions();
services.AddAuthentication();
services.AddAuthorization();
await services.AddCommonServices(configuration, environment);

var app = builder.Build();

app.UseCommonAppConfiguration();
await app.RunAsync();