using JokerChat.BlazorClient.Models;
using System;
using System.Threading.Tasks;

namespace JokerChat.BlazorClient.Communications {
  public class DebugCommunicationChannel : IChatCommunicationChannel {
    public event MessageArrivedHandler OnMessageArrived;

    public Task InitWithConversationIdAsync(string conversationId) {
      StartDebugMessageStream();
      return Task.CompletedTask;
    }

    private async void StartDebugMessageStream() {
      while (true) {
        await Task.Delay(TimeSpan.FromSeconds(1));
        var debugMessage = new JokerMessage {
          Content = "DEBUG",
          DateSent = DateTime.UtcNow,
          Id = Guid.NewGuid(),
          SenderName = "Debug Sender",
          SenderUserId = Guid.NewGuid()
        };

        OnMessageArrived(debugMessage);
      }
    }
  }
}
