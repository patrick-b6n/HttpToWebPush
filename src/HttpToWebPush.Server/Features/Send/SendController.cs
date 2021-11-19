using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HttpToWebPush.Server.Features.Subscriptions;
using HttpToWebPush.Shared.Features.Send;
using HttpToWebPush.Shared.Features.Subscriptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
    public async Task<IActionResult> SendNotification([Required] [FromQuery] Channel channel,
                                                      [Required] [FromBody] SendNotificationDto dto)
    {
        _logger.LogDebug("Received send-notification requests for type '{type}'", channel);

        var message = BuildMessage(channel, dto);

        var subscriptions = _subscriptionService.Find(channel);
        await _pushClient.Send(subscriptions, message);

        return Ok();
    }

    [HttpPost("grafana")]
    public async Task<IActionResult> SendNotification([Required] [FromBody] GrafanaHookDto dto)
    {
        const Channel channel = Channel.Grafana;

        _logger.LogDebug("Received send-notification requests for type '{channel}'", channel);

        var message = BuildMessage(channel, dto);

        var subscriptions = _subscriptionService.Find(channel);
        await _pushClient.Send(subscriptions, message);

        return Ok();
    }

    [HttpPost("toEndpoint")]
    public async Task<IActionResult> SendNotificationToSubscriber([Required] [FromQuery] Channel channel,
                                                                  [Required] [FromBody] SendNotificationToEndpointDto dto)
    {
        _logger.LogDebug("Received send-notification requests for type '{type}'", channel);

        var message = BuildMessage(channel, dto.SendNotificationDto);

        var subscription = _subscriptionService.Find(dto.Endpoint, channel);
        if (subscription == null)
        {
            _logger.LogError("Subscription not available for endpoint '{endpoint}' and type '{subscriptionType}'", dto.Endpoint, channel);
            return BadRequest();
        }

        await _pushClient.Send(subscription, message);
        return Ok();
    }

    private static PushMessageBuilder BuildMessage(Channel channel, SendNotificationDto dto)
    {
        var message = PushMessageBuilder.Create(dto.Title, dto.Body)
                                        .WithLink(dto.Link)
                                        .WithImageForType(channel)
                                        .WithUrgency(dto.Urgency);

        if (dto.TimeToLiveSeconds > 0)
        {
            message.WithTimeToLive(TimeSpan.FromSeconds(dto.TimeToLiveSeconds));
        }

        return message;
    }

    private static PushMessageBuilder BuildMessage(Channel channel, GrafanaHookDto dto)
    {
        var message = PushMessageBuilder.Create($"{dto.Title} {dto.RuleName}", dto.Message)
                                        .WithLink(dto.RuleUrl)
                                        .WithImageForType(channel)
                                        .WithUrgency(Urgency.Normal);
        return message;
    }
}