using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace APIVersionControl
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this._provider = provider;
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "My .NET Api RestFul",
                Version = description.ApiVersion.ToString(),
                Description = "This is my first Api Versioning Control",
                Contact = new OpenApiContact()
                {
                    Email = "ing.maicollcalderonpardo@gmail.com",
                    Name = "Maicoll"
                }
            };

            if (description.IsDeprecated) 
            {
                info.Description += "This Api Version has been deprecated";
            }

            return info;
        }

        public void Configure(SwaggerGenOptions options)
        {
            //Add Swagger Documentation for each API version we have
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }
        }   
      
        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

       

    }
}
