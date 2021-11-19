using System;
using JetBrains.Annotations;

namespace HttpToWebPush.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
[MeansImplicitUse(ImplicitUseTargetFlags.Members)]
public class JsonModelAttribute : Attribute
{
}