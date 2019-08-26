using System;

namespace JokerChat.BlazorClient.Models {
  public class JokerUserData {
    public Guid Id { get; }

    public string Name { get; set; }

    public JokerUserData() {
      Id = Guid.NewGuid();
      Name = "New User";
    }

    public JokerUserData(Guid id, string name) {
      Id = id;
      Name = name ?? throw new ArgumentNullException(nameof(name));
    }
  }
}
