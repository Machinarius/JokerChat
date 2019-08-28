using System;

namespace JokerChat.Common {
  public class RegistrationData {
    public string EndpointHostName { get; set; }
    public int EndpointPort { get; set; }
    public DateTime RegistrationDate { get; set; }
    public Guid SessionId { get; set; }
    public string Username { get; set;}
    public string UserId { get; set; }
  }
}
