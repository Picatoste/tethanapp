using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ThetanCore;
using ThetanSearch;

namespace ThethanApp
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<CookiePolicyOptions>(options =>
      {
              // This lambda determines whether user consent for non-essential cookies is needed for a given request.
              options.CheckConsentNeeded = context => true;
        options.MinimumSameSitePolicy = SameSiteMode.None;
      });


      services.AddOptions();

      services.Configure<ThetanConfig>(Configuration.GetSection("ThetanConfig"));
      services.Configure<ThetanHostedServiceConfig>(Configuration.GetSection("ThetanHostedServiceConfig"));
      services.Configure<ThetanEmailNotificationConfig>(Configuration.GetSection("ThetanEmailNotificationConfig"));

      services.AddSingleton<IThetanNotification, ThetanNotification>();
      services.AddSingleton<ITokenAPIPriceProvider, TokenAPIPriceProvider>();
      services.AddSingleton<IThetanRoiProfitServices, ThetanRoiProfitServices>();
      services.AddSingleton<IThetanAPIProvider, ThetanAPIProvider>();
      services.AddSingleton<IThetanProvider, ThetanProvider>();
      services.AddSingleton<IThetanRepository, ThetanRepository>();
      services.AddSingleton<IDictionary<string, Thetan>, ThethanDBDictionary>();

      //services.AddHostedService<ThetanHostedService>();

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseCookiePolicy();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller=Home}/{action=Thetan}/{id?}");
      });
    }
  }
}
