using Microsoft.Extensions.DependencyInjection;
using System;

namespace JokerChat.HubServer.Orchestration {
  public static class OrchestrationConfiguration {
    public static IServiceCollection AddJokerOrchestrationServices(this IServiceCollection services) {
      if (services is null) {
        throw new ArgumentNullException(nameof(services));
      }

      services.AddSingleton<IMessageOrchestrator, DefaultMessageOrchestrator>();
      return services;
    }
  }
}
