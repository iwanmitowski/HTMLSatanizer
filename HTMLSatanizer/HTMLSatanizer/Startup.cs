using HTMLSatanizer.Data;
using HTMLSatanizer.EmailSender.Contracts;
using HTMLSatanizer.Services;
using HTMLSatanizer.Services.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace HTMLSatanizer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(Configuration["SendGrid:ApiKey"]));
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HTMLSatanizer")));
            services.AddTransient<IHTMLServices, HTMLServices>();
            services.AddTransient<IDataBaseServices, DataBaseServices>();
            services.AddTransient<HttpClient>();

            //DECLARE @count INT = 1;
            //WHILE @count <= 100
            //BEGIN
            //INSERT INTO Site(URL, html, createdon, modifiedon, htmlsatanized, type) VALUES
            //(CONCAT('test.com ', @count), 'hi', CAST(GETUTCDATE() AS DATETIME2), NULL, 'hi', 'URL')

            //SET @count+= 1
            //END;
            //DELETE FROM Site WHERE URL LIKE'test.com%'
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Error/HttpError?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
