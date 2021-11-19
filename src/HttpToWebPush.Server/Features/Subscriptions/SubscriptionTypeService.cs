using HttpToWebPush.Shared.Features.Subscriptions;

namespace HttpToWebPush.Server.Features.Subscriptions;

public class SubscriptionTypeService
{
    private static readonly Dictionary<int, string> SubscriptionTypes = new()
    {
        { (int)Channel.SmartHome, "SmartHome" },
        { (int)Channel.Server, "Server" }
    };

    public Dictionary<int, string> GetSubscriptionTypes()
    {
        return SubscriptionTypes;
    }
}