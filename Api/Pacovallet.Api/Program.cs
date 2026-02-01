using Microsoft.AspNetCore.HttpOverrides;
using Pacovallet.Api.Configurations;
using Pacovallet.Api.Models;
using Pacovallet.Api.Services;
using Pacovallet.Api.Services.Factory;
using Pacovallet.Api.Services.Strategy;
using Pacovallet.Core.Extensions;
using System.Text.Json.Serialization;
using static Pacovallet.Api.Configurations.SwaggerConfig;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IInvoiceProcessingService, InvoiceProcessingService>();
builder.Services.AddScoped<ICreditCardInvoiceProcessorFactory, CreditCardInvoiceProcessorFactory>();
builder.Services.AddScoped<ICreditCardInvoiceProcessor, NubankCreditCardInvoiceProcessor>();

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.AllowAnyOrigin() 
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                          });
});

builder.Services
    .AddControllers(options =>
    {
        options.ModelBinderProviders.Insert(0, new DateTimeCustomConverterForModelBinderProvider());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddOpenApi(options =>
{    
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Test");
        options.RoutePrefix = "swagger";
    });
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
