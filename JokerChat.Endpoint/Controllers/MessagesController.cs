using System;
using System.Threading.Tasks;
using JokerChat.Common;
using JokerChat.Endpoint.HubCommands;
using Microsoft.AspNetCore.Mvc;

namespace JokerChat.Endpoint.Controllers {
  public class MessagesController : Controller {
    private readonly IHubMessageForwarder _messageForwarder;

    public MessagesController(IHubMessageForwarder messageForwarder) {
      _messageForwarder = messageForwarder ?? throw new ArgumentNullException(nameof(messageForwarder));
    }

    [HttpPost]
    public async Task<IActionResult> SendMessageToUser([FromQuery] string userId, [FromBody] JokerMessage message) {
      if (string.IsNullOrEmpty(userId)) {
        return NotFound();
      }

      await _messageForwarder.SendMessageAsync(userId, message);
      return Ok();
    }
  }
}
