using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Squad.AutofacTest.Models;

namespace Squad.AutofacTest
{
    public class Startup
    {
        public ILifetimeScope AutofacContainer { get; private set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        /// <summary>
        /// Autofac Bindings festlegen
        /// </summary>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //when using this binding it works.
            //builder.RegisterType<TodoModelBuilder>().As<ITodoModelBuilder>();

            var applicationAssemblies = LoadApplicationAssemblies("squad.");

            var loadable = applicationAssemblies.SelectMany(a => a.GetLoadableTypes()).ToArray();
            Debug.WriteLine("LOADABLE TYPES:");
            foreach (var t in loadable)
            {
                Debug.WriteLine($"- {t.FullName}");
            }

            builder.RegisterAssemblyTypes(applicationAssemblies.ToArray()).AsImplementedInterfaces();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private List<Assembly> LoadApplicationAssemblies(string partOfassemblyNamespace)
        {
            var ass = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var assemblyList = new List<Assembly>();

            foreach (var file in Directory.GetFiles(ass, "*.dll"))
            {
                try
                {
                    if (file.ToLower().Contains(partOfassemblyNamespace.ToLower()))
                    {
                        assemblyList.Add(Assembly.LoadFile(file));
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return assemblyList;
        }
    }
}
