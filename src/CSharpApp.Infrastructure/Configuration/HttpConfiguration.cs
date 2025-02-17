using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http.Headers;

namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{
    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        var restApiSettings = services.BuildServiceProvider().GetRequiredService<IOptions<RestApiSettings>>().Value;
        var httpClientSettings = services.BuildServiceProvider().GetRequiredService<IOptions<HttpClientSettings>>().Value;
        var authService = () => services.BuildServiceProvider().GetRequiredService<IAuthService>();

        services.AddHttpClient<IProductsService, ProductsService>(ConfigureHttpClient(restApiSettings, httpClientSettings))
            .AddPolicyHandler(PolicySelector(httpClientSettings));

        services.AddHttpClient<ICategoriesService, CategoriesService>(ConfigureHttpClient(restApiSettings, httpClientSettings))
            .AddPolicyHandler(PolicySelector(httpClientSettings));

        services.AddHttpClient<IAuthService, AuthService>(ConfigureHttpClient(restApiSettings, httpClientSettings))
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

    private static Action<IServiceProvider, HttpClient> ConfigureHttpClient(RestApiSettings restApiSettings, HttpClientSettings httpClientSettings)
    {
        return (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
            client.Timeout = TimeSpan.FromSeconds(httpClientSettings.LifeTime);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.ParseAdd("CSharpApp/1.0");
        };
    }

    //private static Action<IServiceProvider, HttpClient> ConfigureHttpClient(RestApiSettings restApiSettings, HttpClientSettings httpClientSettings, Func<IAuthService> authService)
    //{
    //    return (serviceProvider, client) =>
    //    {
    //        var token = authService.Invoke().GetAccessToken().Result;
    //        client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
    //        client.Timeout = TimeSpan.FromSeconds(httpClientSettings.LifeTime);
    //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //        client.DefaultRequestHeaders.UserAgent.ParseAdd("CSharpApp/1.0");
    //        if (token != null)
    //        {
    //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    //        }
    //    };
    //}
}