using Blazored.LocalStorage;
using JokerChat.BlazorClient.Communications;
using JokerChat.BlazorClient.Storage;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace JokerChat.BlazorClient {
  public class Startup {
    public void ConfigureServices(IServiceCollection services) {
      services.AddBlazoredLocalStorage();
      services.AddJokerChatStorage();
      services.AddJokerChatCommunication();
    }

    public void Configure(IComponentsApplicationBuilder app) {
      app.AddComponent<App>("app");
    }
  }
}
