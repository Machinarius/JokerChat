using JokerChat.Common;
using JokerChat.HubServer.Orchestration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JokerChat.HubServer.Controllers {
  public class MessagesController : Controller {
    private readonly IMessageReceptionOrchestrator _receptionOrchestrator;

    [HttpPost]
    public async Task SendMessage(JokerMessage message) {
      await _receptionOrchestrator.ReceiveMessageAsync(message);
    }
  }
}
