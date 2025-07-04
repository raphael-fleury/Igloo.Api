namespace Igloo.Presentation.Extensions;

using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            const string MitLicenseUrl = "https://opensource.org/licenses/MIT";
            const string BearerScheme = "Bearer";

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Igloo API",
                Version = "v1",
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri(MitLicenseUrl)
                }
            });

            c.AddSecurityDefinition(BearerScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = BearerScheme,
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = BearerScheme
                        },
                        Scheme = "bearer",
                        Name = BearerScheme,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            c.UseInlineDefinitionsForEnums();
            
        });

        return services;
    }
} 