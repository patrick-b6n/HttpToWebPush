namespace HttpToWebPush.Shared.Features.Subscriptions;

public static class ChannelIconHelper
{
    public static string GetChannelIconPath(Channel channel)
    {
        return channel switch
        {
            Channel.SmartHome => "/img/channels/smart-home.png",
            Channel.Server => "/img/channels/server.png",
            Channel.Grafana => "/img/channels/grafana.png",
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
    }
}