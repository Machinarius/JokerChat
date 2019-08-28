using JokerChat.Common;
using System.Threading.Tasks;

namespace JokerChat.Endpoint.HubCommands {
  public interface IHubMessageForwarder {
    Task SendMessageAsync(string userId, JokerMessage message);
  }
}
