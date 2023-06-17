using FluentValidation;
using FluentValidation.AspNetCore;
using Led.ContaCorrente.Api.Common;
using Led.ContaCorrente.Api.SwaggerExtensions;
using Led.ContaCorrente.Domain.Abstractions.Services;
using Led.ContaCorrente.Domain.Abstractions.Validadores;
using Led.ContaCorrente.Domain.Atributes;
using Led.ContaCorrente.DomainService;
using Led.ContaCorrente.DomainService.Validadores;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Led.ContaCorrente.Domain.Abstractions.Repository;
using Led.ContaCorrente.DomainRepository;

var builder = WebApplication.CreateBuilder(args);
const string PT_BR = "pt-BR";

CultureInfo ci = new(PT_BR);
CultureInfo.DefaultThreadCurrentCulture = ci;
CultureInfo.DefaultThreadCurrentUICulture = ci;
CultureInfo.CurrentCulture = ci;
CultureInfo.CurrentUICulture = ci;

Thread.CurrentThread.CurrentCulture = ci;
Thread.CurrentThread.CurrentUICulture = ci;

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IMovementRepository, MovementRepository>();

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConfiguracaoVersionamento();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(PT_BR);
    options.SupportedCultures = new List<CultureInfo>
                {
                    ci,
                };
    options.SupportedUICultures = new List<CultureInfo>
                {
                    ci,
                };
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(ValidateModelStateAttribute));
});

var assembly = AppDomain.CurrentDomain.Load(typeof(AccountValidator).Assembly.GetName().FullName);

AssemblyScanner
    .FindValidatorsInAssembly(assembly)
    .ForEach(result => builder.Services.AddScoped(result.InterfaceType, result.ValidatorType));

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(FailFastRequestBehavior<,>));


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AccountValidator>();

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var groupname in descriptionProvider.ApiVersionDescriptions.Select(x => x.GroupName))
        {
            options.SwaggerEndpoint($"{groupname}/swagger.json", groupname.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthorization();


app.MapControllers();

app.Run();
