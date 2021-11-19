using System.ComponentModel.DataAnnotations;
using HttpToWebPush.Server.Common;
using HttpToWebPush.Shared.Features.Subscriptions;

namespace HttpToWebPush.Server.Features.Subscriptions;

public class SubscriptionEntity : Entity
{
    [Required]
    public string Endpoint { get; init; } = null!;

    [Required]
    public string Auth { get; init; } = null!;

    [Required]
    public string P256Dh { get; init; } = null!;

    [Required]
    public Channel Channel { get; init; }
}