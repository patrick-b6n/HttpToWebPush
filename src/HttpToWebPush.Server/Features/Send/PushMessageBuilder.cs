using System;
using HttpToWebPush.Shared.Attributes;
using HttpToWebPush.Shared.Features.Send;
using HttpToWebPush.Shared.Features.Subscriptions;
using Lib.Net.Http.WebPush;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HttpToWebPush.Server.Features.Send;

public class PushMessageBuilder
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

    private PushMessageBuilder(string title, string message)
    {
        _title = title;
        _message = message;
        _urgency = Urgency.Normal;
    }

    public static PushMessageBuilder Create(string title, string message)
    {
        if (string.IsNullOrEmpty(title))
        {
            throw new ArgumentNullException(nameof(title));
        }

        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentNullException(nameof(message));
        }

        return new PushMessageBuilder(title, message);
    }

    public PushMessageBuilder WithLink(string? link)
    {
        if (!string.IsNullOrEmpty(link))
        {
            _link = link;
        }

        return this;
    }

    public PushMessageBuilder WithImageForType(Channel channel)
    {
        _iconUrl = ChannelIconHelper.GetChannelIconPath(channel);

        return this;
    }

    public PushMessageBuilder WithUrgency(Urgency urgency)
    {
        if (_urgency != Urgency.None)
        {
            _urgency = urgency;
        }

        return this;
    }

    public PushMessageBuilder WithTimeToLive(TimeSpan timeToLive)
    {
        _timeToLive = timeToLive;

        return this;
    }

    internal PushMessage Build()
    {
        var model = new PushMessageContent
        (
            Message: _message,
            Title: _title,
            IconUrl: _iconUrl,
            Link: _link
        );

        var message = new PushMessage(JsonConvert.SerializeObject(model, JsonSerializerSettings))
        {
            TimeToLive = _timeToLive?.Seconds
        };

        message.Urgency = _urgency switch
        {
            Urgency.VeryLow => PushMessageUrgency.VeryLow,
            Urgency.Low => PushMessageUrgency.Low,
            Urgency.Normal => PushMessageUrgency.Normal,
            Urgency.High => PushMessageUrgency.High,
            _ => PushMessageUrgency.Normal
        };

        return message;
    }

    [JsonModel]
    private readonly record struct PushMessageContent
    (
        string Message,
        string Title,
        string? IconUrl,
        string? Link
    );
}