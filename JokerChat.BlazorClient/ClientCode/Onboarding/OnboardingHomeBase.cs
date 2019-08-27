using JokerChat.BlazorClient.Models;
using JokerChat.BlazorClient.Storage;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokerChat.BlazorClient.ClientCode.Onboarding {
  public abstract class OnboardingHomeBase : ComponentBase {
    [Inject]
    public IUserDataStore _userDatastore { get; set; }
    [Inject]
    public IUriHelper _uriHelper { get; set; }

    protected override async Task OnInitializedAsync() {
      await base.OnInitializedAsync();
      if (await _userDatastore.UserDataExistsAsync()) {
        ContinueToChatExperience();
      } else {
        ShowOnboardingExperienceUI();
      }
    }

    private void ContinueToChatExperience() {
      _uriHelper.NavigateTo("/chat/general");
    }

    protected abstract void ShowOnboardingExperienceUI();

    protected bool TryToCreateUser(string username, out IEnumerable<string> errorMessages) {
      var errors = new List<string>();
      if (string.IsNullOrEmpty(username) || username.Length < 3) {
        errors.Add("A valid Username must contain at least 3 characters");
      } else {
        if (username.IndexOf(' ') > -1) {
          errors.Add("Usernames may not contain whitespaces");
        }
      }

      errorMessages = errors.AsEnumerable();
      if (!errorMessages.Any()) {
        CreateUser(username);
        return true;
      }

      return false;
    }

    private async void CreateUser(string username) {
      var userObject = new JokerUserData() {
        Name = username
      };
      await _userDatastore.SetCurrentUserDataAsync(userObject);
      ContinueToChatExperience();
    }
  }
}
