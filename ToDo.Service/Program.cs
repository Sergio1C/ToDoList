using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.Hosting.WindowsServices;
using ToDo.DataAccess;
using ToDo.Services.DataBase;
using ToDo.Web;
using Serilog;

try
{
    ClearExistingLogFile("log.txt");

    //1. Add logger setup
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .WriteTo.File("log.txt")
        .CreateBootstrapLogger();

    var options = new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = WindowsServiceHelpers.IsWindowsService()
                                         ? AppContext.BaseDirectory : default,

    };

    var builder = WebApplication.CreateBuilder(options);

    builder.Services.AddLogging(logConfig =>
    {
        logConfig.ClearProviders();
        logConfig.AddSerilog();
    });
    
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.File("log.txt", retainedFileCountLimit: null)
        );

    // Add services to the container.
    builder.Services.AddControllers().AddJsonOptions(
        conf =>
        {
            conf.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
        });
    builder.Services.AddToDoService(builder.Configuration);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Any", policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
    });

    builder.Host.UseWindowsService();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    //app.UseStaticFiles();
    //app.UseEndpoints();
    //app.UseRouting();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.UseCors("Any");
    app.UseSerilogRequestLogging();

    CreateDbIfNotExists(app);

    Log.Logger.Information("Host initilized and running.");
    await app.RunAsync();
    
    static void CreateDbIfNotExists(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ToDoDbContext>();
                var logger = services.GetRequiredService<ILogger<ToDoDbContext>>();
                DbInitializer.Initialize(context, logger);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<ToDoDbContext>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }
    }

    static void ClearExistingLogFile(string fileName)
    {
        var file = new FileInfo(fileName);
        if (file.Exists)
        {
            using var fs = file.Open(FileMode.Truncate);
            fs.Close();
        }
    }
}
catch(Exception ex)
{
    Log.Logger.Fatal(ex, "Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}
