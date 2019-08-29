using JokerChat.HubServer.Subscriptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JokerChat.HubServer.Controllers {
  public class SubscriptionsController : Controller {
    private readonly IConversationSubscriptionsManager _subsManager;

    public SubscriptionsController(IConversationSubscriptionsManager subsManager) {
      _subsManager = subsManager ?? throw new ArgumentNullException(nameof(subsManager));
    }

    [HttpPost]
    public async Task SubscribeToConversation([FromQuery] string userId, [FromQuery] string conversationId) {
      await _subsManager.StoreSubscriptionAsync(userId, conversationId);
    }
  }
}
