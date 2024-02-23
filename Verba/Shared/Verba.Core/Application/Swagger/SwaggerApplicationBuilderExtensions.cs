using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace Verba.Core.Application.Swagger;

public static class SwaggerApplicationBuilderExtensions
{
    public static void UseApiSwagger(this IApplicationBuilder app)
    {
        var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        //app.UseSwaggerUI(o =>
        //{
        //    foreach (var description in provider.ApiVersionDescriptions)
        //    {
        //        o.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        //    }
        //});
    }
}