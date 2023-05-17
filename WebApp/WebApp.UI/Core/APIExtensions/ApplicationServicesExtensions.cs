using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using System.IO.Compression;
using WebApp.DTOs.Auth.Login.Response;
using WebApp.Global.Options;
using WebApp.UI.Core.Authentication.Implementations;

namespace WebApp.UI.Core.APIExtensions
{
	public static partial class ApplicationServicesExtensions
	{
		private static IMvcCoreBuilder AddAdditional(this IMvcCoreBuilder @this, Action<IMvcCoreBuilder> services)
		{
			services?.Invoke(@this);
			return @this;
		}
		public static IMvcCoreBuilder AddCommonMvcMiddleware(this IServiceCollection @this, Action<IMvcCoreBuilder> additional = null)
		{
			return @this
				.AddMvcCore(options =>
				{
					options.ReturnHttpNotAcceptable = true;
					options.RespectBrowserAcceptHeader = true;
					options.OutputFormatters.RemoveType<StreamOutputFormatter>();
					options.OutputFormatters.RemoveType<StringOutputFormatter>();
				})
				.AddApiExplorer()
				.AddAuthorization()
				.AddDataAnnotations()
				.AddFormatterMappings()
				.AddCors(options =>
				{
					options.AddPolicy("AllowedAll", builder => builder
					.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod()
					//.SetIsOriginAllowed(origin => true)
					//.AllowCredentials()
					);

				})
				.AddAdditional(additional);
		}
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			//services.AddMvc(opt =>
			//{
			//    opt.Filters.Add(typeof(InputValidationFilter));
			//});

			services.AddMemoryCache();

			return services;
		}
		public static IServiceCollection AddCommonOptions(this IServiceCollection @this, IConfiguration configuration)
		{
			@this.AddOptions();

			try
			{
				@this.Configure<ApplicationOptions>(option => configuration.GetSection(nameof(ApplicationOptions)).Bind(option));
				@this.AddSingleton(x => x.GetRequiredService<IOptions<ApplicationOptions>>().Value);
			}
			catch { }

			try
			{
				@this.Configure<AuthenticationOptions>(option => configuration.GetSection(nameof(AuthenticationOptions)).Bind(option));
				@this.AddSingleton(x => x.GetRequiredService<IOptions<AuthenticationOptions>>().Value);
			}
			catch { }

			try
			{
				@this.Configure<ApiOptions>(option => configuration.GetSection(nameof(ApiOptions)).Bind(option));
				@this.AddSingleton(x => x.GetRequiredService<IOptions<ApiOptions>>().Value);
			}
			catch { }

			return @this;
		}
		public static IServiceCollection AddCommonResponseCompression(this IServiceCollection @this)
		{
			@this.AddResponseCompression(options =>
			{

				options.Providers.Clear();
				options.Providers.Add<GzipCompressionProvider>();
				options.MimeTypes = new[]
				{
					"text/json",
					"text/html",
					"text/plain",
					"application/json"
				};

			}).Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

			return @this;
		}
		public static IServiceCollection AddApiBehaviorOptions(this IServiceCollection @this)
		{
			@this.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressConsumesConstraintForFormFileParameters = true;
				options.SuppressInferBindingSourcesForParameters = true;
				options.SuppressModelStateInvalidFilter = true;
			});

			return @this;
		}
		public static IServiceCollection AddCommonHttps(this IServiceCollection @this)
		{
			var appOptions = @this.BuildServiceProvider().GetRequiredService<ApplicationOptions>();

			if (appOptions.Hosting.UseHttps)
			{
				@this.AddHsts(options =>
				{
					options.Preload = true;
					options.IncludeSubDomains = true;
					options.MaxAge = TimeSpan.FromHours(24);
				});

				@this.AddHttpsRedirection(options =>
				{
					options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
					options.HttpsPort = appOptions.Hosting.HttpsPort;
				});
			}

			return @this;
		}

		public static IServiceCollection AddTokenProviders(this IServiceCollection @this) =>
			@this.AddTransient<TokenProvider>();


		public static IServiceCollection AddAuthenticationHeader(this IServiceCollection @this)
		{
			@this.AddAuthentication("Bearer").AddJwtBearer(options =>
			{
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						//var cookieValue = context.Request.Cookies["X-Access-Token"];

						var tokenValue = context.HttpContext.Session.GetString("X-Access-Token");

						if (!string.IsNullOrWhiteSpace(tokenValue))
						{
							var tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseDto>(tokenValue);

							if (tokenData != null)
							{
								// SET THE TOKEN IN AUTHORIZATION HEADER
								context.Token = tokenData.Token;
							}
						}
						return Task.CompletedTask;
					}
				};
			});

			return @this;
		}
	}
}
