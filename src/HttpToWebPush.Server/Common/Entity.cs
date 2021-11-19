using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace HttpToWebPush.Server.Common;

public class Entity
{
    [Key]
    [UsedImplicitly]
    public int Id { get; private set; }
}