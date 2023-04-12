using Microsoft.EntityFrameworkCore;
using CoronaBackend.Health;
using System.Reflection;
using NLog.Web;
using CoronaBackend.Data;

namespace CoronaBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");

                var builder = WebApplication.CreateBuilder(args);
                
                builder.Services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                    loggingBuilder.AddDebug();
                });

                builder.WebHost.UseNLog();
                builder.Services.AddControllers();

                builder.Services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {
                    options.EnableAnnotations();
                    options.SupportNonNullableReferenceTypes();
                    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                });

                builder.Services.AddHealthChecks().AddCheck<CustomHealth>("api");

                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                    });
                });

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    var scope = app.Services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();

                    app.UseSwagger();
                    app.UseSwaggerUI(options =>
                    {
                        options.DefaultModelsExpandDepth(-1);
                    });
                };

                app.UseCors();

                app.UseRouting();

                app.UseEndpoints(endpoints => endpoints.MapControllers());

                app.UseHealthChecks("/health");
                
                app.Run();

                logger.Debug("Program started");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }

        }
    }
}