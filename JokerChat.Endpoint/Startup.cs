using JokerChat.Endpoint.ClientCommands;
using JokerChat.Endpoint.HubCommands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JokerChat.Endpoint {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddControllersWithViews();
      services.AddSignalR();
      services.AddSingleton<IUserIdProvider, JokerUserIdProvider>();

      services.AddCors(options => {
        options.AddDefaultPolicy(BuildCORSPolicy);
      });

      services.Configure<EndpointConfiguration>(Configuration.GetSection("EndpointConfiguration"));

      services.AddJokerHubCommandServices();
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

      app.UseCors();
      app.UseStaticFiles();
      app.UseRouting();

      app.UseEndpoints(endpoints => {
        endpoints
          .MapHub<JokerSignalRHub>("/jokerhub")
          .RequireCors(BuildCORSPolicy);
      });

      app.UseEndpoints(endpoints => {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }

    private void BuildCORSPolicy(CorsPolicyBuilder builder) {
      builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    }
  }
}
