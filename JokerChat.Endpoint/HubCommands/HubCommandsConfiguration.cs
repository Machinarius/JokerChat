using Microsoft.Extensions.DependencyInjection;
using System;

namespace JokerChat.Endpoint.HubCommands {
  public static class HubCommandsConfiguration {
    public static IServiceCollection AddJokerHubCommandServices(this IServiceCollection services) {
      if (services is null) {
        throw new ArgumentNullException(nameof(services));
      }

      services.AddTransient<IHubMessageForwarder, DefaultMessageForwarder>();
      return services;
    }
  }
}
