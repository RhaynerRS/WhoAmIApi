using Richter.WhoAmIApi.IoC.Settings;
using Elastic.Channels;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using System.Diagnostics;
using ElasticsearchSinkOptions = Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions;

namespace Richter.WhoAmIApi.IoC.Config
{
    public static class SerilogConfig
    {
        public static IServiceCollection AddSerilog(this IServiceCollection services, IHostEnvironment environment, SerilogSettings settings)
        {
            services.AddSerilog((sp, options) => ConfigurarSerilog(sp, environment, options, settings));
            return services;
        }

        private static void ConfigurarSerilog(IServiceProvider serviceProvider,
                                              IHostEnvironment environment,
                                              LoggerConfiguration builder,
                                              SerilogSettings settings)
        {
            var aplicacaoSettings = serviceProvider.GetRequiredService<IOptions<ApplicationSettings>>().Value;

            builder
                .MinimumLevel.Information()
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);

            builder
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithExceptionDetails()
                .Enrich.WithSpan()
                .Enrich.FromLogContext();

            builder.WriteTo.Console();

            if (!Debugger.IsAttached)
            {
                var identificadorAplicao = aplicacaoSettings.Nome.ToLower().Replace(" ", "-");
                var ambiente = environment.EnvironmentName.ToLower().Replace("development", "dev");
                var urlNode = new Uri(settings.ElasticSearchUrl);

                // Atualmente nosso ambiente de logs em produção usa ElasticSearch, enquanto os ambientes de desenvolvimento e homologação usam OpenSearch.
                // Por conta dessa diferença é necessário que cada ambiente escreva logs usando estratégias deferentes.

                if (environment.IsEnvironment("Prod"))
                {
                    builder.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(urlNode)
                    {
                        IndexFormat = $"dotnet-api-{ambiente}-{identificadorAplicao}-{{0:yyyy.MM}}",
                    });
                }
                else
                {
                    builder.WriteTo.Elasticsearch([urlNode], opts =>
                    {
                        opts.DataStream = new DataStreamName($"dotnet-api-{ambiente}", identificadorAplicao, DateTime.Today.ToString("yyyy.MM"));
                        opts.BootstrapMethod = BootstrapMethod.None;

                        opts.ConfigureChannel = channelOpts =>
                        {
                            channelOpts.BufferOptions = new BufferOptions { ExportMaxConcurrency = 10 };
                        };
                    });
                }
            }
        }
    }
}