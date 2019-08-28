using JokerChat.Common;
using System.Threading.Tasks;

namespace JokerChat.HubServer.Orchestration {
  public interface IMessageOrchestrator {
    Task ReceiveMessageAsync(JokerMessage message);
  }
}
