using System;

namespace JokerChat.Common {
  public class JokerMessage {
    public string Id { get; set; }
    public string SenderId { get; set; }
    public string SenderUsername { get; set; }
    public DateTime DateSent { get; set; }
    public string Content { get; set; }
    public string ConversationId { get; set; }
  }
}
