using Verba.Abstractions.Application.Modules;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Verba.Core.Application.Swagger
{
    /// <summary>
    /// Helper functions for configuring Swagger services.
    /// </summary>
    public static class SwaggerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds swagger
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {                
                using (var provider = services.BuildServiceProvider())
                {
                    //var versionDescriptionProvider = provider.GetRequiredService<IApiVersionDescriptionProvider>();
                    //var moduleProvider = provider.GetRequiredService<IModuleProvider>();
                
                    //foreach (var description in versionDescriptionProvider.ApiVersionDescriptions)
                    //{
                    //    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                    //}

                    //foreach (var assembly in moduleProvider.ApplicationPartAssemblies)
                    //{
                    //    options.IncludeXmlComments(assembly.Location.Replace(".dll", ".xml"));
                    //}
                    
                    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey

                    });
                }
            });
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {            
            var info = new OpenApiInfo
            {
                Title = $"Verba API {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),

            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}