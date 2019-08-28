using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;

namespace JokerChat.Endpoint.ClientCommands {
  public class JokerUserIdProvider : IUserIdProvider {
    public string GetUserId(HubConnectionContext connection) {
      string authJson = null;
      var request = connection.GetHttpContext().Request;
      if (request.Headers.TryGetValue("Authentication", out var authHeaderValue)) {
        authJson = authHeaderValue.ToString();
      } else {
        var queryStringAuth = request.Query["access_token"].ToString();
        if (!string.IsNullOrEmpty(queryStringAuth)) {
          authJson = queryStringAuth;
        }
      }

      if (string.IsNullOrEmpty(authJson)) {
        throw new UnauthorizedAccessException("No Authentication info was detected");
      }

      var authPayload = JsonConvert.DeserializeObject<RegistrationToken>(authJson);
      return authPayload.UserId;
    }
  }
}
