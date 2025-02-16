using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        var restApiSettings = services.BuildServiceProvider().GetRequiredService<IOptions<RestApiSettings>>().Value;
        var httpClientSettings = services.BuildServiceProvider().GetRequiredService<IOptions<HttpClientSettings>>().Value;
        services.AddHttpClient<IProductsService, ProductsService>(ConfigureHttpClient(restApiSettings, httpClientSettings, restApiSettings.Products!))
        .AddPolicyHandler(PolicySelector(httpClientSettings));

        return services;
    }

    private static Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> PolicySelector(HttpClientSettings httpClientSettings)
    {
        return (serviceProvider, request) =>
                    HttpPolicyExtensions.HandleTransientHttpError()
                        .WaitAndRetryAsync(httpClientSettings.RetryCount,
                            retryAttempt => TimeSpan.FromMilliseconds(httpClientSettings.SleepDuration * retryAttempt));
    }

    private static Action<IServiceProvider, HttpClient> ConfigureHttpClient(RestApiSettings restApiSettings, HttpClientSettings httpClientSettings, string path)
    {
        return (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(restApiSettings.BaseUrl! + path + "/");
            client.Timeout = TimeSpan.FromSeconds(httpClientSettings.LifeTime);
        };
    }
}