using JokerChat.Common;
using System.Threading.Tasks;

namespace JokerChat.HubServer.Registrations {
  public interface IRegistrationsManager {
    Task StoreRegistrationAsync(RegistrationData registrationData);
    Task<RegistrationData> GetRegistrationForUserId(string userId);
  }
}
