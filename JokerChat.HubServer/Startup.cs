using JokerChat.HubServer.Orchestration;
using JokerChat.HubServer.Registrations;
using JokerChat.HubServer.Subscriptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Linq;

namespace JokerChat.HubServer {
  public class Startup {
    private const string CORSPolicyName = "JokerCORSPolicy";

    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddControllersWithViews();
      

      services.AddHttpContextAccessor();
      services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

      services.AddJokerRegistrationServices();
      services.AddJokerOrchestrationServices();
      services.AddJokerSubscriptionServices();

      services.Configure<ServicesConfiguration>(Configuration.GetSection("Services"));
      
      var provider = services.BuildServiceProvider();
      var configuration = provider.GetService<IOptions<ServicesConfiguration>>().Value;
      var allowedCorsHosts = configuration.AllowedCORSOrigins.ToArray();
      services.AddCors(options => options.AddPolicy(CORSPolicyName, builder =>
        builder.WithOrigins(allowedCorsHosts)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        app.UseHttpsRedirection();
      }

      app.UseCors(CORSPolicyName);
      app.UseRouting();
      app.UseStaticFiles();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
