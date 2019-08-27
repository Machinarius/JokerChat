using System;

namespace JokerChat.BlazorClient.Models {
  public class JokerMessage {
    public Guid Id { get; set; }
    public DateTime DateSent { get; set; }
    public Guid SenderUserId { get; set; }
    public string SenderName { get; set; }
    public string Content { get; set; }
  }
}
