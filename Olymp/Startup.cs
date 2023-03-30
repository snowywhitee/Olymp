using MediatR;
using Microsoft.OpenApi.Models;
using Olymp.Repository;
using Olymp.Repository.InMemoryImpl;

namespace Olymp;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<InMemoryImplementation>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();

        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new  OpenApiInfo
            {
                Version = "v1",
                Title = "Olymp",
                Description = "Description"
            });
            c.CustomSchemaIds(SwashbuckleSchemaHelper.GetSchemaId);
            c.DescribeAllParametersInCamelCase();

            //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //c.IncludeXmlComments(xmlPath);
        });

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Olymp service API"));
        
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private static class SwashbuckleSchemaHelper
    { 
        private static readonly Dictionary<string, int> _schemaNames = new();

        public static string GetSchemaId(Type type)
        {
            string id = type.Name;
            if (!_schemaNames.ContainsKey(id))
            {
                _schemaNames.Add(id, 0);
            }

            int count = _schemaNames[id] + 1;
            _schemaNames[id] = count;

            return type.Name + (count > 1 ? count.ToString() : "");
        }
    }
}