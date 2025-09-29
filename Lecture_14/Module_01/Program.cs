using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;
using FluentValidation;
using Module_01;
using Module_01.Filters;
using Module_01.Requests;
using FluentValidation.Results;

var builder = WebApplication.CreateBuilder(args);

/*

builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

*/

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt => opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

var app = builder.Build();

/*

app.MapControllers();

app.MapPost("/api/products", (CreateProductRequest request) => Results.Created($"/api/products/{Guid.NewGuid()}", request))
   .Validate<CreateProductRequest>();

*/

app.MapPost("/api/products", (CreateProductRequest request) => Results.Created($"/api/products/{Guid.NewGuid()}", request))
   .AddEndpointFilter<ValidationFilter<CreateProductRequest>>();

app.Run();