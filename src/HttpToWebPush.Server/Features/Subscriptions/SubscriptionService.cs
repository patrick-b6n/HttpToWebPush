using HttpToWebPush.Shared.Features.Subscriptions;
using Lib.Net.Http.WebPush;

namespace HttpToWebPush.Server.Features.Subscriptions;

public class SubscriptionService
{
    private readonly PushCenterDbContext _dbContext;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(PushCenterDbContext dbContext, ILogger<SubscriptionService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public Task Save(PushSubscription pushSubscription, Channel channel)
    {
        _logger.LogInformation("Adding '{SubscriptionType}' subscription for '{Endpoint}'", channel, pushSubscription.Endpoint);

        if (_dbContext.Subscriptions.Any(s => s.Endpoint == pushSubscription.Endpoint && s.Channel == channel))
        {
            return Task.CompletedTask;
        }

        var subscription = new SubscriptionEntity
        {
            Endpoint = pushSubscription.Endpoint,
            Auth = pushSubscription.GetKey(PushEncryptionKeyName.Auth),
            P256Dh = pushSubscription.GetKey(PushEncryptionKeyName.P256DH),
            Channel = channel
        };

        _dbContext.Add(subscription);
        return _dbContext.SaveChangesAsync();
    }

    public SubscriptionEntity? Find(string endpoint, Channel type)
    {
        return _dbContext.Subscriptions.SingleOrDefault(s => s.Endpoint == endpoint && s.Channel == type);
    }

    public IEnumerable<Channel> Find(string endpoint)
    {
        return _dbContext.Subscriptions.Where(s => s.Endpoint == endpoint).Select(s => s.Channel);
    }

    public IEnumerable<SubscriptionEntity> Find(Channel channel)
    {
        return _dbContext.Subscriptions.Where(s => s.Channel == channel);
    }

    public Task Delete(PushSubscription pushSubscription, Channel channel)
    {
        _logger.LogInformation("Deleting '{SubscriptionType}' subscription for '{Endpoint}'", channel, pushSubscription.Endpoint);

        var subscriptions = _dbContext.Subscriptions
                                      .Where(s => s.Endpoint == pushSubscription.Endpoint && s.Channel == channel);

        _dbContext.Subscriptions.RemoveRange(subscriptions);
        return _dbContext.SaveChangesAsync();
    }
}