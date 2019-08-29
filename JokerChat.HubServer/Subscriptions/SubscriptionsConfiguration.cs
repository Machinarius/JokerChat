using Microsoft.Extensions.DependencyInjection;
using System;

namespace JokerChat.HubServer.Subscriptions {
  public static class SubscriptionsConfiguration {
    public static IServiceCollection AddJokerSubscriptionServices(this IServiceCollection services) {
      if (services is null) {
        throw new ArgumentNullException(nameof(services));
      }

      services.AddSingleton<IConversationSubscriptionsManager, RedisConversationSubscriptionManager>();
      return services;
    }
  }
}
