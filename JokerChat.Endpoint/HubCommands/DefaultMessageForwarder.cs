using System;
using System.Threading.Tasks;
using JokerChat.Common;
using JokerChat.Endpoint.ClientCommands;
using Microsoft.AspNetCore.SignalR;

namespace JokerChat.Endpoint.HubCommands {
  public class DefaultMessageForwarder : IHubMessageForwarder {
    private readonly IHubContext<JokerSignalRHub> _hubContext;

    public DefaultMessageForwarder(IHubContext<JokerSignalRHub> hubContext) {
      _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    public async Task SendMessageAsync(string userId, JokerMessage message) {
      var client = _hubContext.Clients.User(userId);
      if (client == null) {
        System.Diagnostics.Debug.WriteLine("Could not find a client with Id: " + userId);
        return;
      }

      await client.SendAsync("receiveMessage", message);
    }
  }
}
