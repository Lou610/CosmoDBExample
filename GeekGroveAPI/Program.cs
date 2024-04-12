using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace GeekGroveAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? MyAllowSpecificOrigins = "HomeLab";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:7239", "http://localhost:7239", "http://localhost:4200", "https://localhost:4200", "http://localhost:3100", "https://localhost:3100", "https://auth.geekgrove.xyz/", "https://auth.geekgrove.xyz/", "https://api.geekgrove.xyz/", "http://api.geekgrove.xyz/", "http://home.geekgrove.xyz/", "http://home.geekgrove.xyz/")
                                      .WithHeaders("Access-Control-Allow-Origin: *")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .WithMethods("PUT", "DELETE", "GET", "PATCH", "POST")
                                      .SetIsOriginAllowedToAllowWildcardSubdomains();
                                  });

            });

            builder.Logging.AddConsole();

           

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            // Add ApiExplorer to discover versions
            builder.Services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "GeekGrove API V1" });
                c.SwaggerDoc("v2", new OpenApiInfo { Version = "v2", Title = "GeekGrove API V2" });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseExceptionHandler("/Error");
            app.UseHsts();

            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (ApiVersionDescription apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
                {
                    string isDeprecated = apiVersionDescription.IsDeprecated ? " (DEPRECATED)" : string.Empty;
                    options.SwaggerEndpoint($"{builder.Configuration["PathBase"]}/swagger/{apiVersionDescription.GroupName}/swagger.json",
                        $"{apiVersionDescription.GroupName.ToUpperInvariant()}{isDeprecated}");
                }
            });

            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
