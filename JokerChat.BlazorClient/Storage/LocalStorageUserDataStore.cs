using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using JokerChat.BlazorClient.Models;

namespace JokerChat.BlazorClient.Storage {
  public class LocalStorageUserDataStore : IUserDataStore {
    private const string UserDataKey = "JokerChat.UserData";

    private readonly ILocalStorageService _localStorage;

    public LocalStorageUserDataStore(ILocalStorageService localStorage) {
      _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }

    public async Task ClearUserDataAsync() {
      await _localStorage.RemoveItemAsync(UserDataKey);
    }

    public async Task<JokerUserData> GetCurrentUserDataAsync() {
      var userData = await _localStorage.GetItemAsync<JokerUserData>(UserDataKey);
      if (userData == null) {
        throw new InvalidOperationException("There is no User data in the browser's storage");
      }

      return userData;
    }

    public async Task SetCurrentUserDataAsync(JokerUserData userData) {
      if (userData is null) {
        throw new ArgumentNullException(nameof(userData));
      }

      await _localStorage.SetItemAsync(UserDataKey, userData);
    }

    public async Task<bool> UserDataExistsAsync() {
      var userData = await _localStorage.GetItemAsync<JokerUserData>(UserDataKey);
      return userData != null;
    }
  }
}
