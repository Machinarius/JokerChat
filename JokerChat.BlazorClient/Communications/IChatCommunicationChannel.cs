using JokerChat.BlazorClient.Models;
using System.Threading.Tasks;

namespace JokerChat.BlazorClient.Communications {
  public delegate void MessageArrivedHandler(JokerMessage incomingMessage);

  public interface IChatCommunicationChannel {
    event MessageArrivedHandler OnMessageArrived;

    Task InitWithConversationIdAsync(string conversationId);
  }
}
