using JokerChat.BlazorClient.Models;
using System.Threading.Tasks;

namespace JokerChat.BlazorClient.Storage {
  public interface IUserDataStore {
    Task<bool> UserDataExistsAsync();
    Task<JokerUserData> GetCurrentUserDataAsync();
    Task ClearUserDataAsync();
    Task SetCurrentUserDataAsync(JokerUserData userData);
  }
}
