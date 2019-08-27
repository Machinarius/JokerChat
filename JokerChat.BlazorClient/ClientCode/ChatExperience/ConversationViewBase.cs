using JokerChat.BlazorClient.Communications;
using JokerChat.BlazorClient.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JokerChat.BlazorClient.ClientCode.ChatExperience {
  public abstract class ConversationViewBase : ComponentBase {
    protected List<JokerMessage> MessagesToDisplay { get; private set; }

    [Inject]
    public IChatCommunicationChannel CommunicationChannel { get; set; }

    protected async Task InitializeWithConversationIdAsync(string conversationId) {
      CommunicationChannel.OnMessageArrived += OnMessageArrived;
      await CommunicationChannel.InitWithConversationIdAsync(conversationId);
    }

    private void OnMessageArrived(JokerMessage incomingMessage) {
      MessagesToDisplay.Add(incomingMessage);
      StateHasChanged();
    }
  }
}
