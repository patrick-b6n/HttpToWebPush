using HttpToWebPush.Server.Common;
using HttpToWebPush.Server.Features.Subscriptions;
using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.Extensions.Options;

namespace HttpToWebPush.Server.Features.Send;

public class PushClient : PushServiceClient
{
    private readonly PushCenterDbContext _dbContext;

    private readonly ILogger<PushClient> _logger;
    private readonly VapidAuthentication _vapidAuthentication;

    public PushClient(IHttpClientFactory client,
                      IOptions<PushApiOptions> options,
                      PushCenterDbContext dbContext,
                      ILogger<PushClient> logger) : base(client.CreateClient())
    {
        _logger = logger;
        _dbContext = dbContext;

        _vapidAuthentication = new VapidAuthentication(options.Value.PublicKey, options.Value.PrivateKey)
        {
            Subject = options.Value.Subject
        };
    }

    public async Task Send(IEnumerable<SubscriptionEntity> subscriptions, PushMessageFactory pushMessageFactory)
    {
        var pushSubs = subscriptions.Select(Map).ToList();

        _logger.LogDebug("Sending PushMessage to {Count} subscribers", pushSubs.Count);

        foreach (var subscription in pushSubs)
        {
            try
            {
                await RequestPushMessageDeliveryAsync(subscription, pushMessageFactory.Build(), _vapidAuthentication);
            }
            catch (PushServiceClientException e)
            {
                _logger.LogError(e, "Failed to send Notification to '{Endpoint}'", subscription.Endpoint);

                if (e.Message == "Gone")
                {
                    _logger.LogInformation("Removing gone subscription '{Endpoint}'", subscription.Endpoint);
                    var toDelete = _dbContext.Subscriptions.Where(s => s.Endpoint == subscription.Endpoint);
                    _dbContext.RemoveRange(toDelete);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }

    public Task Send(SubscriptionEntity subscription, PushMessageFactory pushMessageFactory)
    {
        return Send(new[] { subscription }, pushMessageFactory);
    }

    private static PushSubscription Map(SubscriptionEntity subscription)
    {
        var pushSubscription = new PushSubscription();
        pushSubscription.Endpoint = subscription.Endpoint;
        pushSubscription.SetKey(PushEncryptionKeyName.Auth, subscription.Auth);
        pushSubscription.SetKey(PushEncryptionKeyName.P256DH, subscription.P256Dh);
        return pushSubscription;
    }
}