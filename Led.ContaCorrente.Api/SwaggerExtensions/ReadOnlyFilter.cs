using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Led.ContaCorrente.Api.SwaggerExtensions
{
    [ExcludeFromCodeCoverage]
    public class ReadOnlyFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties == null) return;

            foreach (var schemaProperty in schema.Properties)
                CheckIsReadOnly(schemaProperty, context);
        }

        private static void CheckIsReadOnly(KeyValuePair<string, OpenApiSchema> schemaProperty, SchemaFilterContext context)
        {
            var property = context.Type.GetProperty(schemaProperty.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null) return;

            schemaProperty.Value.ReadOnly = false;

            var attr = property.GetCustomAttributes<ReadOnlyAttribute>(false).SingleOrDefault();

            if (attr != null && attr.IsReadOnly)
                SetReadOnly(schemaProperty);
        }

        private static void SetReadOnly(KeyValuePair<string, OpenApiSchema> schemaProperty)
        {
            if (schemaProperty.Value.Reference != null)
            {
                schemaProperty.Value.AllOf = new List<OpenApiSchema>()
                        {
                            new OpenApiSchema()
                            {
                                Reference = schemaProperty.Value.Reference
                            }
                        };

                schemaProperty.Value.Reference = null;
            }

            schemaProperty.Value.ReadOnly = true;
        }
    }
}
