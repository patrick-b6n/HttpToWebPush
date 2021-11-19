namespace HttpToWebPush.Server.Common;

public class PushApiOptions
{
    public string Subject { get; init; } = null!;
    public string PublicKey { get; init; } = null!;
    public string PrivateKey { get; init; } = null!;
}