using System.Collections.Generic;
using System.Threading.Tasks;

namespace JokerChat.HubServer.Subscriptions {
  public interface IConversationSubscriptionsManager {
    Task StoreSubscriptionAsync(string userId, string conversationId);
    Task<IEnumerable<string>> GetSubscribedIdsAsync(string conversationId);
  }
}
