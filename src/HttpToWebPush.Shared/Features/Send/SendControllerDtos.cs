namespace HttpToWebPush.Shared.Features.Send;

public enum Urgency
{
    None = 0,
    VeryLow = 1,
    Low = 2,
    Normal = 3,
    High = 4
}

public readonly record struct SendNotificationDto
(
    string Body,
    string Title,
    string? Link,
    Urgency Urgency,
    int TimeToLiveSeconds
);

public readonly record struct SendNotificationToEndpointDto
(
    string Endpoint,
    SendNotificationDto SendNotificationDto
);