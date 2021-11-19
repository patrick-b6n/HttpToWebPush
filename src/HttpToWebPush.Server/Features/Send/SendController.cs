using HttpToWebPush.Server.Features.Subscriptions;
using HttpToWebPush.Shared.Features.Send;
using HttpToWebPush.Shared.Features.Subscriptions;
using Microsoft.AspNetCore.Mvc;

namespace HttpToWebPush.Server.Features.Send;

[ApiController]
[Route("api/[controller]")]
public class SendController : ControllerBase
{
    private readonly ILogger<SendController> _logger;
    private readonly PushClient _pushClient;
    private readonly SubscriptionService _subscriptionService;

    public SendController(SubscriptionService subscriptionService,
                          PushClient pushClient,
                          ILogger<SendController> logger)
    {
        _subscriptionService = subscriptionService;
        _pushClient = pushClient;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification([FromQuery] Channel? channel,
                                                      [FromBody] SendNotificationDto? dto)
    {
        if (channel == null) throw new ArgumentNullException(nameof(channel));
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        _logger.LogDebug("Received send-notification requests for type '{type}'", channel);

        var message = BuildMessage(channel.Value, dto);

        var subscriptions = _subscriptionService.Find(channel.Value);
        await _pushClient.Send(subscriptions, message);

        return Ok();
    }

    [HttpPost("toEndpoint")]
    public async Task<IActionResult> SendNotificationToSubscriber([FromQuery] Channel? channel,
                                                                  [FromBody] SendNotificationToEndpointDto? dto)
    {
        if (channel == null) throw new ArgumentNullException(nameof(channel));
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        _logger.LogDebug("Received send-notification requests for type '{type}'", channel);

        var message = BuildMessage(channel.Value, dto.SendNotificationDto);

        var subscription = _subscriptionService.Find(dto.Endpoint, channel.Value);
        if (subscription == null)
        {
            _logger.LogError("Subscription not available for endpoint '{endpoint}' and type '{subscriptionType}'", dto.Endpoint, channel);
            return BadRequest();
        }

        await _pushClient.Send(subscription, message);
        return Ok();
    }

    private static PushMessageFactory BuildMessage(Channel type, SendNotificationDto dto)
    {
        var message = PushMessageFactory.Create(dto.Title, dto.Body)
                                        .WithLink(dto.Link)
                                        .WithImageForType(type)
                                        .WithUrgency(dto.Urgency);

        if (dto.TimeToLiveSeconds > 0)
        {
            message.WithTimeToLive(TimeSpan.FromSeconds(dto.TimeToLiveSeconds));
        }

        return message;
    }
}