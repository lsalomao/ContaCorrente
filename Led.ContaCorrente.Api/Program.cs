using FluentValidation;
using Led.ContaCorrente.Api.Common;
using Led.ContaCorrente.Api.Extensions;
using Led.ContaCorrente.Api.SwaggerExtensions;
using Led.ContaCorrente.Domain.Abstractions.Services;
using Led.ContaCorrente.Domain.Requests;
using Led.ContaCorrente.DomainService;
using Led.ContaCorrente.DomainService.Validadores;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddControllers().AddValidators();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConfiguracaoVersionamento();

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
