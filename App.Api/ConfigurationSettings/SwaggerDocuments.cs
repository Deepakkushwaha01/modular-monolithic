namespace App.API.Settings;

public static class SwaggerDocuments
{
    public const string Common = "common";
    public const string Identity = "identity";
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class SwaggerModuleAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
