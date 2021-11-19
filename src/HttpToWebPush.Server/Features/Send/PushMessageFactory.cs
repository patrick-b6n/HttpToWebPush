using System.Diagnostics.CodeAnalysis;
using HttpToWebPush.Shared.Features.Send;
using HttpToWebPush.Shared.Features.Subscriptions;
using Lib.Net.Http.WebPush;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HttpToWebPush.Server.Features.Send;

public class PushMessageFactory
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    private readonly string _message;
    private readonly string _title;
    private string? _iconUrl;
    private string? _link;
    private TimeSpan? _timeToLive;
    private Urgency _urgency;

    private PushMessageFactory(string title, string message)
    {
        _title = title;
        _message = message;
        _urgency = Urgency.Normal;
    }

    public static PushMessageFactory Create(string title, string message)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentNullException(nameof(message));
        }

        return new PushMessageFactory(title, message);
    }

    public PushMessageFactory WithLink(string link)
    {
        if (!string.IsNullOrEmpty(link))
        {
            _link = link;
        }

        return this;
    }

    public PushMessageFactory WithImageForType(Channel channel)
    {
        _iconUrl = ChannelIconHelper.GetChannelIconPath(channel);

        return this;
    }

    public PushMessageFactory WithUrgency(Urgency urgency)
    {
        if (_urgency != Urgency.None)
        {
            _urgency = urgency;
        }

        return this;
    }

    public PushMessageFactory WithTimeToLive(TimeSpan timeToLive)
    {
        _timeToLive = timeToLive;

        return this;
    }

    internal PushMessage Build()
    {
        var model = new PushMessageContentModel
        {
            Title = _title,
            Message = _message,
            IconUrl = _iconUrl,
            Link = _link
        };

        var message = new PushMessage(JsonConvert.SerializeObject(model, JsonSerializerSettings))
        {
            TimeToLive = _timeToLive?.Seconds
        };

        switch (_urgency)
        {
            case Urgency.VeryLow:
                message.Urgency = PushMessageUrgency.VeryLow;
                break;
            case Urgency.Low:
                message.Urgency = PushMessageUrgency.Low;
                break;
            case Urgency.Normal:
                message.Urgency = PushMessageUrgency.Normal;
                break;
            case Urgency.High:
                message.Urgency = PushMessageUrgency.High;
                break;
        }

        return message;
    }

    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    private class PushMessageContentModel
    {
        public string Message { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string? IconUrl { get; init; }
        public string? Link { get; init; }
    }
}