using Module_01.Data;
using Module_01.Formatters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt => { 
                opt.ReturnHttpNotAcceptable = true;
                opt.OutputFormatters.Add(new PlainTextTableOutputFormatters());
                })
                .AddXmlSerializerFormatters()
                .AddNewtonsoftJson(opt => { opt.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore; })
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; });

builder.Services.AddSingleton<ProductRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();
