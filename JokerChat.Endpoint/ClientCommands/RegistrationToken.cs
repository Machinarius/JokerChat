﻿using System;

namespace JokerChat.Endpoint.ClientCommands {
  public class RegistrationToken {
    public string UserId { get; set; }
    public string Username { get; set; }
    public Guid SessionId { get; set;}
  }
}
