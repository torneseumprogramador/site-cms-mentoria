using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using site_cms.Models;

namespace site_cms
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            JToken jAppSettings = JToken.Parse(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "appsettings.json")));
            var host = jAppSettings["host"];
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    HttpClient http = new HttpClient();
                    var response = await http.GetAsync($"{host}/api/paginas/home.json");
                    if(response.IsSuccessStatusCode)
                    {
                        var result  = response.Content.ReadAsStringAsync().Result;
                        var pagina = JsonConvert.DeserializeObject<Pagina>(result);
                        await context.Response.WriteAsync(pagina.Conteudo);
                    }
                    else
                    {
                        await context.Response.WriteAsync("Pagina não encontrada");
                    }
                });

                endpoints.MapGet("/{slug}", async context =>
                {
                    HttpClient http = new HttpClient();
                    var url = $"{host}/api/paginas" + context.Request.Path.Value + ".json";
                    Console.WriteLine(url);
                    var response = await http.GetAsync(url);
                    if(response.IsSuccessStatusCode)
                    {
                        var result  = response.Content.ReadAsStringAsync().Result;
                        var pagina = JsonConvert.DeserializeObject<Pagina>(result);
                        await context.Response.WriteAsync(pagina.Conteudo);
                    }
                    else
                    {
                        await context.Response.WriteAsync("Pagina não encontrada");
                    }
                });
            });
        }
    }
}
