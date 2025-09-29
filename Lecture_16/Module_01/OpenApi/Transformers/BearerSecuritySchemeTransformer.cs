using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Module_01.OpenApi.Transformers;

internal sealed class BearerSecuritySchemeTransformer: IOpenApiDocumentTransformer, IOpenApiOperationTransformer
{
    private const string schemId = JwtBearerDefaults.AuthenticationScheme;

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new();
        document.Components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>();

        document.Components.SecuritySchemes[schemId] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = schemId
            }
        };

        return Task.CompletedTask;
    }
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any())
        {
            operation.Security ??= [];

            var key = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference()
            };


            key.Reference.Type = ReferenceType.SecurityScheme;
            key.Reference.Id = schemId;

            var requirement = new OpenApiSecurityRequirement
            {
                {key, []}
            };

            operation.Security.Add(requirement);
        }

        return Task.CompletedTask;
    }
}