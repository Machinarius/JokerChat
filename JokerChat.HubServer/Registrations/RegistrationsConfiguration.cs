using Microsoft.Extensions.DependencyInjection;
using System;

namespace JokerChat.HubServer.Registrations {
  public static class RegistrationsConfiguration {
    public static IServiceCollection AddJokerRegistrationServices(this IServiceCollection services) {
      if (services is null) {
        throw new ArgumentNullException(nameof(services));
      }

      services.AddSingleton<IRegistrationsManager, RedisRegistrationManager>();
      return services;
    }
  }
}
