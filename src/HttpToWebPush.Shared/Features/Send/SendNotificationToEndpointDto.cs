namespace HttpToWebPush.Shared.Features.Send;

public class SendNotificationToEndpointDto
{
    public string Endpoint { get; init; } = null!;

    public SendNotificationDto SendNotificationDto { get; init; } = null!;
}