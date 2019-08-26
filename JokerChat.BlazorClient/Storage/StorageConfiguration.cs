using Microsoft.Extensions.DependencyInjection;
using System;

namespace JokerChat.BlazorClient.Storage {
  public static class StorageConfiguration {
    public static IServiceCollection AddJokerChatStorage(this IServiceCollection services) {
      if (services is null) {
        throw new ArgumentNullException(nameof(services));
      }

      services = services.AddSingleton<IUserDataStore, LocalStorageUserDataStore>();
      return services;
    }
  }
}
