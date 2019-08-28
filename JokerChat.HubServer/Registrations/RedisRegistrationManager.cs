using JokerChat.Common;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokerChat.HubServer.Registrations {
  public class RedisRegistrationManager : IRegistrationsManager {
    private const string RegistrationsKey = "JokerChat.Registrations";

    private readonly ServicesConfiguration _servicesConfig;
    private ConnectionMultiplexer _redisConnection;
    private IDatabase _redisDb;

    public RedisRegistrationManager(IOptions<ServicesConfiguration> servicesOptions) {
      _servicesConfig = servicesOptions.Value;
    }

    private async Task InitAsync() {
      if (_redisConnection == null) {
        _redisConnection = await ConnectionMultiplexer.ConnectAsync(_servicesConfig.RedisHost);
        _redisDb = _redisConnection.GetDatabase();
      }
    }

    public async Task StoreRegistrationAsync(RegistrationData registrationData) {
      if (registrationData is null) {
        throw new ArgumentNullException(nameof(registrationData));
      }

      await InitAsync();
      var currentRegistrations = await GetCurrentRegistrationsAsync();
      if (currentRegistrations.Any(reg => reg.SessionId == registrationData.SessionId)) {
        return;
      }

      var matchingRegistrations = currentRegistrations.Where(reg => reg.UserId == registrationData.UserId).ToArray();
      foreach (var registration in matchingRegistrations) {
        currentRegistrations.Remove(registration);
      }
      currentRegistrations.Add(registrationData);

      var updatedRegistrationsJson = JsonConvert.SerializeObject(currentRegistrations);
      await _redisDb.StringSetAsync(RegistrationsKey, updatedRegistrationsJson);
    }

    public async Task<RegistrationData> GetRegistrationForUserId(string userId) {
      var registrations = await GetCurrentRegistrationsAsync();
      var matchingRegistration = registrations.FirstOrDefault(reg => reg.UserId == userId);
      return matchingRegistration;
    }

    private async Task<List<RegistrationData>> GetCurrentRegistrationsAsync() {
      string currentRegistrationsJson;
      var currentRegistrationsValue = await _redisDb.StringGetAsync(RegistrationsKey);
      if (!currentRegistrationsValue.IsNull) {
        currentRegistrationsJson = currentRegistrationsValue;
      } else {
        currentRegistrationsJson = "[]";
      }

      var currentRegistrations = JsonConvert.DeserializeObject<List<RegistrationData>>(currentRegistrationsJson);
      return currentRegistrations;
    }
  }
}
