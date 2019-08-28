using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JokerChat.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace JokerChat.Endpoint.ClientCommands {
  public class JokerSignalRHub : Hub {
    private readonly IOptions<EndpointConfiguration> _configuration;

    public JokerSignalRHub(IOptions<EndpointConfiguration> configuration) {
      _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task PerformAnnouncement(RegistrationToken token) {
      var config = _configuration.Value;
      var registrationData = new RegistrationData() {
        EndpointPort = config.SelfHttpPort,
        RegistrationDate = DateTime.UtcNow,
        SessionId = token.SessionId,
        UserId = token.UserId,
        Username = token.Username
      };

      var registrationJson = JsonConvert.SerializeObject(registrationData);
      using (var httpClient = new HttpClient()) {
        var request = new HttpRequestMessage(HttpMethod.Post, config.HubUrl + "/Registrations/AnnounceRegistration") { 
          Content = new StringContent(registrationJson, Encoding.UTF8, "application/json")
        };

        var response = await httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode) {
          System.Diagnostics.Debug.WriteLine("Non-Successful status code for registration");
        }
      }
    }

    public void SubscribeToConversation(ConversationSubRequest request) {

    }
  }
}
