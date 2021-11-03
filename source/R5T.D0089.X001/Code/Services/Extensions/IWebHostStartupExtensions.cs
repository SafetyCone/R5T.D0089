using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.T0063;
using R5T.T0072;
using R5T.T0073;
using R5T.T0074;


namespace R5T.D0089.X001
{
    public static class IWebHostStartupExtensions
    {
        public static TWebHostBuilder UseWebHostStartup<TWebHostStartup, TWebHostBuilder>(this TWebHostBuilder webHostBuilder,
            IServiceAction<TWebHostStartup> webHostStartupAction)
            where TWebHostStartup : IWebHostStartup
            where TWebHostBuilder :
            IHasConfigureConfiguration<TWebHostBuilder>,
            IHasConfigureServices<TWebHostBuilder>,
            IHasConfigureApplication<TWebHostBuilder>
        {
            var webStartupServiceProvider = Instances.ServiceOperator.GetServiceInstance(
                webHostStartupAction,
                out var webHostStartup);

            webHostBuilder.UseWebHostStartup(webHostStartup);

            // Add a ConfigureServices() call (added after the startup instance's ConfigureServices() call) that will dispose of the startup service provider after the startup service will have been used.
            webHostBuilder.ConfigureServices(_ =>
            {
                webStartupServiceProvider.Dispose(); // Chose Dispose() over DisposeAsync().
            });

            return webHostBuilder;
        }

        public static TWebHostBuilder UseWebHostStartup<TWebHostStartup, TWebHostBuilder>(this TWebHostBuilder webHostBuilder,
            IServiceProvider webHostStartupServiceProvider)
            where TWebHostStartup : IWebHostStartup
            where TWebHostBuilder :
            IHasConfigureConfiguration<TWebHostBuilder>,
            IHasConfigureServices<TWebHostBuilder>,
            IHasConfigureApplication<TWebHostBuilder>
        {
            var webHostStartup = webHostStartupServiceProvider.GetRequiredService<TWebHostStartup>();

            webHostBuilder.UseWebHostStartup(webHostStartup);

            return webHostBuilder;
        }

        public static TWebHostBuilder UseWebHostStartup<TWebHostStartup, TWebHostBuilder>(this TWebHostBuilder webHostBuilder,
            TWebHostStartup webHostStartup)
            where TWebHostStartup : IWebHostStartup
            where TWebHostBuilder :
            IHasConfigureConfiguration<TWebHostBuilder>,
            IHasConfigureServices<TWebHostBuilder>,
            IHasConfigureApplication<TWebHostBuilder>
        {
            webHostBuilder
                .ConfigureConfiguration(configurationBuilder =>
                {
                    SyncOverAsyncHelper.ExecuteTaskSynchronously(webHostStartup.ConfigureConfiguration(configurationBuilder));
                })
                .ConfigureServices(services =>
                {
                    SyncOverAsyncHelper.ExecuteTaskSynchronously(webHostStartup.ConfigureServices(services));
                })
                .ConfigureApplication(applicationBuilder =>
                {
                    SyncOverAsyncHelper.ExecuteTaskSynchronously(webHostStartup.ConfigureApplication(applicationBuilder));
                })
                ;

            return webHostBuilder;
        }
    }
}