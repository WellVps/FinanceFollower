using BaseService.Models;
using Microsoft.OpenApi.Any;
using BaseDomain.ValueObjects;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;

namespace BaseApi.Extensions;

public static class SwaggerGenOptionsExtensions
{
    public static void AddValueObjectTypes(this SwaggerGenOptions options)
    {
        options.MapType<SortBy>(() => new OpenApiSchema
        {
            Type = "string",
            Example = new OpenApiString("columnName|asc_or_desc")
        });

        options.MapType<AddressCode>(() => new OpenApiSchema
        {
            Type = "string"            
        });
    }
}