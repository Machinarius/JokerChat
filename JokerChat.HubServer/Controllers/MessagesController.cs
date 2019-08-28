using JokerChat.Common;
using JokerChat.HubServer.Orchestration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JokerChat.HubServer.Controllers {
  public class MessagesController : Controller {
    private readonly IMessageOrchestrator _receptionOrchestrator;

    public MessagesController(IMessageOrchestrator receptionOrchestrator) {
      _receptionOrchestrator = receptionOrchestrator ?? throw new ArgumentNullException(nameof(receptionOrchestrator));
    }

    [HttpPost]
    public async Task SendMessage([FromBody] JokerMessage message) {
      await _receptionOrchestrator.ReceiveMessageAsync(message);
    }
  }
}
