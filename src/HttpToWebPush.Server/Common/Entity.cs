using System.ComponentModel.DataAnnotations;

namespace HttpToWebPush.Server.Common;

public class Entity
{
    [Key]
    public int Id { get; private set; }
}