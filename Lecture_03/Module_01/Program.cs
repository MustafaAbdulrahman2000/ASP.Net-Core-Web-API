using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<RequestTracker>();

builder.Services.AddScoped<ServiceA>();
builder.Services.AddScoped<ServiceB>();

/*

builder.Services.AddHttpClient();
builder.Services.AddTransient<IWeatherClient, WeatherClient>();
builder.Services.Add(new ServiceDescriptor
(   
    typeof(IWeatherService),
    typeof(WeatherService),
    ServiceLifetime.Transient
));

builder.Services.AddWeatherServices();

builder.Services.AddTransient<IDependency, Dependency1>();
builder.Services.AddTransient<IDependency, Dependency2>();
builder.Services.TryAddTransient<IDependency, Dependency1>();

builder.Services.AddKeyedTransient<IDependency, Dependency1>("v1");
builder.Services.AddKeyedTransient<IDependency, Dependency2>("v2");

builder.Services.AddTransient<IPaymentProvider>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    return config["PaymentProvider"] == "Stripe" ? new StripePayment() : new PaypalPayment();
});


builder.Services.AddTransient<DbInitializer>();

*/

var app = builder.Build();

/*

app.MapGet("/weather/city/{cityName}", (string cityName, IWeatherService weatherService, ILogger<Program> logger) => 
{
    logger.LogInformation("Fetching temperature for city : {city}", cityName);

    var data = weatherService.GetWeatherInfo(cityName);

    logger.LogInformation("Fetched temperature for city : {city}", cityName);

    return Results.Ok(data);
});

app.MapGet("/single", ([FromKeyedServices("v1")] IDependency dependency) =>
{
    return Results.Ok(dependency.DoSomething());
});

app.MapGet("/multiple", (IEnumerable<IDependency> dependencies) =>
{
    var response = string.Empty;

    foreach (var dependency in dependencies)
    {
        response += dependency.DoSomething() + "\n";
    }

    return Results.Ok(response);
});

app.MapGet("/idependency-registrations", (IServiceProvider sp) =>
{
    var servicesRegisteredCount = sp.GetServices<IDependency>();

    return Results.Ok(servicesRegisteredCount.Count());
});

app.MapGet("/v2", ([FromKeyedServices("v2")] IDependency dependency) =>
{
    return Results.Ok(dependency.DoSomething());
});

app.MapGet("/pay/{amount}", (decimal amount, IPaymentProvider paymentProvider) =>
{
    var response = paymentProvider.Pay(amount);

    return Results.Ok(response);
});

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;

    var initializer = sp.GetRequiredService<DbInitializer>();

    initializer.Initialize();
}

*/

app.MapGet("/check", (ServiceA serviceA, ServiceB serviceB) => 
{
    return Results.Ok(new 
    {
        A = serviceA.GetInfo(),
        B = serviceB.GetInfo()
    });
});

app.Run();

/*

public interface IWeatherService
{
    string GetWeatherInfo(string cityName);
}

public class WeatherService: IWeatherService
{
    private readonly IWeatherClient _weatherClient;

    public WeatherService(IWeatherClient weatherClient)
    {
        _weatherClient = weatherClient;
    }

    public string GetWeatherInfo(string cityName)
    {
        return _weatherClient.GetWeatherInfo(cityName);
    }
}

public interface IWeatherClient
{
    string GetWeatherInfo(string cityName);
}

public class WeatherClient: IWeatherClient
{
    private readonly HttpClient _httpClient;
    
    public WeatherClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string GetWeatherInfo(string cityName)
    {
        // Some external http call
        return $"Weather for {cityName} is {Random.Shared.Next(-10, 40)} C.";
    }
}


public interface IDependency
{
    string DoSomething();
}

public class Dependency1: IDependency
{
    public string DoSomething()
    {
        return "Dependency version [1]";
    }
}

public class Dependency2: IDependency
{
    public string DoSomething()
    {
        return "Dependency version [2]";
    }
}

public interface IPaymentProvider
{
    string Pay(decimal amount);
}

public class StripePayment: IPaymentProvider
{
    public string Pay(decimal amount)
    => $"Payment of ${amount} was processed using stripe !!";
}

public class PaypalPayment: IPaymentProvider
{
    public string Pay(decimal amount)
    => $"Payment of ${amount} was processed using paypal !!";
}

public class DbInitializer
{
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(ILogger<DbInitializer> logger)
    {
        _logger = logger;
    }

    public void Initialize()
    {
        // Logic for seeding 1000 items

        _logger.LogInformation("1000 items were seeded successfully.");
    }
}

*/

public class RequestTracker
{
    public string TrackerId = Guid.NewGuid().ToString()[..8];
}

public class ServiceA(RequestTracker tracker)
{
    public string GetInfo()
    => $"A => {tracker.TrackerId}";
}

public class ServiceB(RequestTracker tracker)
{
    public string GetInfo()
    => $"B => {tracker.TrackerId}";
}