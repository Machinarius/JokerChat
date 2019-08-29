using System.Collections.Generic;

namespace JokerChat.HubServer {
  public class ServicesConfiguration {
    public string RedisHost { get; set; }
    public IEnumerable<string> AllowedCORSOrigins { get; set; }
  }
}
