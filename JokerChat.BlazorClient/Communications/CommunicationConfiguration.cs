using Microsoft.Extensions.DependencyInjection;

namespace JokerChat.BlazorClient.Communications {
  public static class CommunicationConfiguration {
    public static IServiceCollection AddJokerChatCommunication(this IServiceCollection services) {
      if (services is null) {
        throw new System.ArgumentNullException(nameof(services));
      }

      services = services.AddTransient<IChatCommunicationChannel, DebugCommunicationChannel>();
      return services;
    }
  }
}
