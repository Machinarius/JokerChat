using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JokerChat.Common;
using JokerChat.HubServer.Registrations;
using JokerChat.HubServer.Subscriptions;
using Newtonsoft.Json;

namespace JokerChat.HubServer.Orchestration {
  public class DefaultMessageOrchestrator : IMessageOrchestrator {
    private readonly IRegistrationsManager _registrationsManager;
    private readonly IConversationSubscriptionsManager _subsManager;

    public DefaultMessageOrchestrator(IRegistrationsManager registrationsManager, IConversationSubscriptionsManager subsManager) {
      _registrationsManager = registrationsManager ?? throw new ArgumentNullException(nameof(registrationsManager));
      _subsManager = subsManager ?? throw new ArgumentNullException(nameof(subsManager));
    }

    public async Task ReceiveMessageAsync(JokerMessage message) {
      if (message is null) {
        throw new ArgumentNullException(nameof(message));
      }

      var targetUserIds = await _subsManager.GetSubscribedIdsAsync(message.ConversationId);
      if (!targetUserIds.Any()) {
        return;
      }

      targetUserIds = targetUserIds.Distinct().ToArray();
      var messageJson = JsonConvert.SerializeObject(message);
      using (var httpClient = new HttpClient()) {
        foreach (var target in targetUserIds) {
          var registration = await _registrationsManager.GetRegistrationForUserId(target);
          if (registration == null) {
            continue;
          }

          var hostname = registration.EndpointHostName;
          if (hostname == "::1" || hostname == "127.0.0.1") { // IPv6 hostname
            hostname = "localhost";
          }

          var targetUrl = $"http://{hostname}:{registration.EndpointPort}/Messages/SendMessageToUser?userId={target}";
          using var request = new HttpRequestMessage(HttpMethod.Post, targetUrl) {
            Content = new StringContent(messageJson, Encoding.UTF8, "application/json")
          };

          try {
            var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
          } catch (Exception) {
            System.Diagnostics.Debug.WriteLine("Could not send a message to a user");
          }
        }
      }
    }
  }
}
