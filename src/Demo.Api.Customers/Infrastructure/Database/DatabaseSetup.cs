using Demo.Api.Customers.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Api.Customers.Infrastructure.Database;

public static class DatabaseSetup
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
        => services.AddScoped<ICustomersRepository, CustomersRepository>();
}
