using JokerChat.Common;
using System.Threading.Tasks;

namespace JokerChat.HubServer.Orchestration {
  public interface IMessageReceptionOrchestrator {
    Task ReceiveMessageAsync(JokerMessage message);
  }
}
