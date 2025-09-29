using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Module_01.OpenApi.Transformers;

internal sealed class versionInfoTransformer: IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var version = context.DocumentName;
        
        document.Info.Version = version;
        document.Info.Title = $"Project API {version}";

        return Task.CompletedTask;
    }
}
