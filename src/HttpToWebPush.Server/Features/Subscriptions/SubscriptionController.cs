using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using HttpToWebPush.Server.Common;
using HttpToWebPush.Shared.Features.Subscriptions;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HttpToWebPush.Server.Features.Subscriptions;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly PushApiOptions _pushApiOptions;
    private readonly SubscriptionService _subscriptionService;

    public SubscriptionController(SubscriptionService subscriptionService,
                                  IOptions<PushApiOptions> pushCenterOptions)
    {
        _subscriptionService = subscriptionService;
        _pushApiOptions = pushCenterOptions.Value;
    }

    [HttpDelete]
    public async Task<IActionResult> Unsubscribe([Required] [FromQuery] Channel channel,
                                                 [Required] [FromBody] PushSubscription subscription)
    {
        await _subscriptionService.Delete(subscription, channel);

        return Ok();
    }

    [HttpGet("public-key")]
    public IActionResult GetPublicKey()
    {
        return Content(_pushApiOptions.PublicKey, "text/plain");
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe([Required] [FromQuery] Channel channel,
                                               [Required] [FromBody] PushSubscription subscription)
    {
        await _subscriptionService.Save(subscription, channel);

        return Ok();
    }

    /// <summary>
    ///     Passing URLs via query parameters can lead to encoding issues. It's safer to send it in the body.
    /// </summary>
    [HttpPost("find")]
    public IActionResult Find([FromBody] string endpoint)
    {
        if (endpoint == null)
        {
            throw new ArgumentNullException(nameof(endpoint));
        }

        return new JsonResult(_subscriptionService.Find(endpoint));
    }
}