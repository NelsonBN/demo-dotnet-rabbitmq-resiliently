using Demo.Api.Payments.Domain.Payers;
using Demo.Api.Payments.Domain.Payments;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Api.Payments.Infrastructure.Database;

public static class DatabaseSetup
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
        => services.AddScoped<IPayersRepository, PayersRepository>()
                   .AddScoped<IPaymentsRepository, PaymentsRepository>();
}
