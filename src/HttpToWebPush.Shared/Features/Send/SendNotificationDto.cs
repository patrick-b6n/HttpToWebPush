using System.ComponentModel.DataAnnotations;

namespace HttpToWebPush.Shared.Features.Send;

public class SendNotificationDto
{
    [Required]
    public string Body { get; init; } = null!;

    [Required]
    public string Title { get; init; } = null!;

    public string Link { get; init; } = null!;

    public Urgency Urgency { get; init; }

    public int TimeToLiveSeconds { get; init; }
}