using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokerChat.HubServer.Subscriptions {
  public class RedisConversationSubscriptionManager : IConversationSubscriptionsManager {
    private const string SubscriptionsKey = "JokerChat.Subscriptions";

    private readonly ServicesConfiguration _servicesConfig;
    private ConnectionMultiplexer _redisConnection;
    private IDatabase _redisDb;

    public RedisConversationSubscriptionManager(IOptions<ServicesConfiguration> config) {
      if (config is null) {
        throw new ArgumentNullException(nameof(config));
      }

      _servicesConfig = config.Value;
    }

    private async Task InitAsync() {
      if (_redisConnection == null) {
        _redisConnection = await ConnectionMultiplexer.ConnectAsync(_servicesConfig.RedisHost);
        _redisDb = _redisConnection.GetDatabase();
      }
    }

    public async Task<IEnumerable<string>> GetSubscribedIdsAsync(string conversationId) {
      await InitAsync();
      var currentSubs = await GetConversationSubscriptionsJsonAsync(conversationId);
      return currentSubs;
    }

    public async Task StoreSubscriptionAsync(string userId, string conversationId) {
      await InitAsync();
      var currentSubs = await GetConversationSubscriptionsJsonAsync(conversationId);
      var modifiedSubs = currentSubs.ToList();
      if (!modifiedSubs.Any(subUserId => subUserId == userId)) {
        modifiedSubs.Add(userId);
      }

      await StoreConversationSubscriptionsAsync(modifiedSubs, conversationId);
    }

    private async Task<IEnumerable<string>> GetConversationSubscriptionsJsonAsync(string conversationId) {
      string subsJson;
      var subsValue = await _redisDb.StringGetAsync(SubscriptionsKey + "." + conversationId);
      if (!subsValue.IsNull) {
        subsJson = subsValue;
      } else {
        subsJson = "[]";
      }

      var currentSubs = JsonConvert.DeserializeObject<IEnumerable<string>>(subsJson);
      return currentSubs;
    }

    private async Task StoreConversationSubscriptionsAsync(IEnumerable<string> modifiedSubs, string conversationId) {
      if (modifiedSubs is null) {
        throw new ArgumentNullException(nameof(modifiedSubs));
      }

      var subsJson = JsonConvert.SerializeObject(modifiedSubs);
      await _redisDb.StringSetAsync(SubscriptionsKey + "." + conversationId, subsJson);
    }
  }
}
