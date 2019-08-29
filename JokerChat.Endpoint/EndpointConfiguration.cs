using System.Collections.Generic;

namespace JokerChat.Endpoint {
  public class EndpointConfiguration {
    public string HubUrl { get; set; }
    public int OwnHttpPort { get; set; }
    public IEnumerable<string> AllowedCORSOrigins { get; set; }
  }
}
