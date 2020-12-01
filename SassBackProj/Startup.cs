using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System.IO;
using SassBackProj.Middleware;
using SassBackProj.Services.FoodService;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;

namespace SassBackProj
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebSocketServerConnectionManager(); // web socket
            services.AddControllers(); // api default
            services.AddDirectoryBrowser(); // know 
            services.AddScoped<IFoodService, FoodService>(); // api/
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // instancja. 
            //int x = 0;
            //Task.Run(() => { x = 2; invokeAsync() });




            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseWebSockets();

            app.UseWebSocketServer(); // /ws

            app.UseRouting(); // http

            var options = new DefaultFilesOptions(); // /
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(options);


            #region description
            /*
            UseDefaultFiles must be called before UseStaticFiles to serve the default file. UseDefaultFiles is a URL rewriter that doesn't serve the file.

           With UseDefaultFiles, requests to a folder in wwwroot search for:

           default.htm
           default.html
           index.htm
           index.html
           The first file found from the list is served as though the request were the fully qualified URI. The browser URL continues to reflect the URI requested.
           */
            #endregion 



            app.UseStaticFiles(); // using static files from wwwroot-directory. contain only HTML/CSS/JS 

            app.UseStaticFiles(new StaticFileOptions // public directory, made to upload and dowlnoad files
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "public")),
                RequestPath = "/public"
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "public")),
                RequestPath = "/public"
            });





            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/mango", async context =>
                {
                    IMongoDatabase db;
                    var client = new MongoDB.Driver.MongoClient();
                    db = client.GetDatabase("SASS_Project");
                    string tabName = "users";
                    var records = db.GetCollection<user>(tabName);

                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(records, records.GetType()));
                });
                endpoints.MapControllers();
            });
        }
    }
    class user
    {
        public string email { get; set; }
        public string gender { get; set; }
        public string phone_number { get; set; }
        public string birthdate { get; set; }
        public location location { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string title { get; set; }
        public string picture { get; set; }

    }
    class location
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int postcode { get; set; }
    }
}
