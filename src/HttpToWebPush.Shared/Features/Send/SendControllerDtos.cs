namespace HttpToWebPush.Shared.Features.Send;

public enum Urgency
{
    None = 0,
    VeryLow = 1,
    Low = 2,
    Normal = 3,
    High = 4
}

public record SendNotificationDto
(
    string Body,
    string Title,
    string? Link,
    Urgency Urgency,
    int TimeToLiveSeconds
);

public record SendNotificationToEndpointDto
(
    string Endpoint,
    SendNotificationDto SendNotificationDto
);