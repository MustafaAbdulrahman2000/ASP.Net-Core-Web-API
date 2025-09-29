using System.Text.RegularExpressions;

namespace Module_01.Transformers;

public class SlugifyTransformer: IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        return value is null
               ? null
               : Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2")
               .Replace(" ", "-")
               .ToLowerInvariant();
    }
}