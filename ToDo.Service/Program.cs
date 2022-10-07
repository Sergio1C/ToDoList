using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.Hosting.WindowsServices;
using ToDo.DataAccess;
using ToDo.Services.DataBase;
using ToDo.Web;

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() 
                                     ? AppContext.BaseDirectory : default,   
                                     
};

var builder = WebApplication.CreateBuilder(options);

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

CreateDbIfNotExists(app);

await app.RunAsync();

static void CreateDbIfNotExists(IHost host)
{    
    using (var scope = host.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ToDoDbContext>();
            DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}
