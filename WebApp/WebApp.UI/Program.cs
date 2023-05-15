using Microsoft.AspNetCore.Http.Features;
using WebApp.Global.Options;
using WebApp.UI.Core.APIExtensions;
using WebApp.UI.Core.Middlewares;

var builder = WebApplication.CreateBuilder(args);


try
{
    #region Configure Builder

    builder.WebHost.ConfigureAppConfiguration((builderContext, config) =>
    {
        AddConfigurations(config, builderContext.HostingEnvironment);
    })
    .UseKestrel(options =>
    {
        options.AddServerHeader = false;
        //options.AllowSynchronousIO = true; // Remove this after making all the endpoints as Async
    });

    #endregion

    #region Configure Services
    ConfigureServices(builder);
    #endregion

    #region Configure Middleware and Pipeline

    //Configure Middleware and Pipeline
    var app = builder.Build();

    Configure(app);

    #endregion

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Host Terminated Unexpectedly :" + ex);
}
finally
{
    NLog.LogManager.Shutdown();
    Console.WriteLine("Final Statement Execution");
}

static void AddConfigurations(IConfigurationBuilder builder, IWebHostEnvironment environment)
{
    string appSettingsFileName = $"appsettings.{environment.EnvironmentName}.json";

    string authenticationSettingsFileName = $"authentication.{environment.EnvironmentName}.json";
    string apiOptionsFileName = $"apioptions.{environment.EnvironmentName}.json";

    var dirPath = Directory.GetCurrentDirectory();

    builder.SetBasePath(dirPath)
        .AddAppJsonFile((Microsoft.AspNetCore.Hosting.IWebHostEnvironment)environment, appSettingsFileName, false, true)
        .AddSecretJsonFile((Microsoft.AspNetCore.Hosting.IWebHostEnvironment)environment, authenticationSettingsFileName, false, true)
        .AddSecretJsonFile((Microsoft.AspNetCore.Hosting.IWebHostEnvironment)environment, apiOptionsFileName, false, true)
        .AddEnvironmentVariables();
}


static void Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else // Configure the HTTP request pipeline.
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseExceptionMiddleware();
    // app.UseStatusCodePagesWithReExecute("/errors/{0}");
    app.UseAppSettings();
    app.UseStaticFiles();
    //app.UseStaticFiles(new StaticFileOptions()
    //{
    //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    //    RequestPath = new PathString("/Resources")
    //});

    ////Enable directory browsing
    //app.UseDirectoryBrowser(new DirectoryBrowserOptions
    //{
    //    FileProvider = new PhysicalFileProvider(
    //                Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    //    RequestPath = "/Resources"
    //});


    app.UseRouting();

    app.UseCors("AllowedAll");

    app.UseAuthorization();

    app.UseAuthenticationMiddleware();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
    //app.MapControllerRoute(
    //    name: "default",
    //    pattern: "{controller=Auth}/{action=Index}/{id?}");
}

static void ConfigureServices(WebApplicationBuilder builder)
{
    //Configure Services
    var applicationOptions = new ApplicationOptions();
    builder.Configuration.Bind(nameof(ApplicationOptions), applicationOptions);

    var authenticationOptions = new AuthenticationOptions();
    builder.Configuration.Bind(nameof(AuthenticationOptions), authenticationOptions);


    var apiOptions = new ApiOptions();
    builder.Configuration.Bind(nameof(ApiOptions), apiOptions);

    var provider = builder.Services.BuildServiceProvider();

    var netCoreLogger = provider.GetService<ILogger<Program>>();
    //SQL Connection

    _ = builder.Services.AddSingleton(builder.Configuration)
        .AddCommonOptions(builder.Configuration)
        .AddAuthenticationHeader()
        .AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger), netCoreLogger)
        .AddCommonHttps()
        .AddHttpContextAccessor()
        .AddAndConfigureHttpClients()
        .AddCommonResponseCompression()
        .AddRouting(options => options.LowercaseUrls = true)
        .AddApiBehaviorOptions()
        .AddTokenProviders()
        .AddApplicationServices()
        .AddApplicationServicesBusinessDI()
        .AddCommonMvcMiddleware().Services
        .AddControllers();

    builder.Services.Configure<FormOptions>(o =>
    {
        o.ValueLengthLimit = int.MaxValue;
        o.MultipartBodyLengthLimit = int.MaxValue;
        o.MemoryBufferThreshold = int.MaxValue;
    });

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    builder.Services.AddMvc().AddRazorRuntimeCompilation();
}

