using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JokerChat.Common;
using JokerChat.HubServer.Registrations;
using Newtonsoft.Json;

namespace JokerChat.HubServer.Orchestration {
  public class DefaultMessageOrchestrator : IMessageOrchestrator {
    private readonly IRegistrationsManager _registrationsManager;

    public DefaultMessageOrchestrator(IRegistrationsManager registrationsManager) {
      _registrationsManager = registrationsManager ?? throw new ArgumentNullException(nameof(registrationsManager));
    }

    public async Task ReceiveMessageAsync(JokerMessage message) {
      if (message is null) {
        throw new ArgumentNullException(nameof(message));
      }

      // ECHO for now
      var registration = await _registrationsManager.GetRegistrationForUserId(message.SenderId);
      if (registration == null) {
        return;
      }

      var messageJson = JsonConvert.SerializeObject(message);
      using (var httpClient = new HttpClient()) {
        var hostname = registration.EndpointHostName;
        if (hostname == "::1" || hostname == "127.0.0.1") { // IPv6 hostname
          hostname = "localhost";
        }

        var targetUrl = $"http://{hostname}:{registration.EndpointPort}/Messages/SendMessageToUser?userId={message.SenderId}";
        var request = new HttpRequestMessage(HttpMethod.Post, targetUrl) {
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
