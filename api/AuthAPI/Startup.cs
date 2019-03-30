/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Startup
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * Full notice : https://github.com/amirkhaniansev/tyche/tree/master/LICENSE
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/

using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using Tyche.MailSevice;
using Tyche.LoggerService;
using Tyche.PasswordHasherService;
using Tyche.CodeGeneratorService;
using Tyche.AuthAPI.Constant;
using Tyche.AuthAPI.Authentication;
using Tyche.TycheApiUtilities.Middleware;

namespace Tyche.AuthAPI
{
    /// <summary>
    /// Class for startup of Authentication API
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Gets configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Creates new instance of <see cref="Startup"/>
        /// </summary>
        /// <param name="configuration">configurarion</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.ConfigureApp();
        }
        
        /// <summary>
        /// Configures services
        /// </summary>
        /// <param name="services">services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddIdentityServer()
                .AddSigningCredential(RsaProvider.GenerateSigningCredential())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddProfileService<ProfileService>();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();            
        }

        /// <summary>
        /// Configures API
        /// </summary>
        /// <param name="app">app</param>
        /// <param name="env">environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseIdentityServer();
        }

        /// <summary>
        /// Configures app
        /// </summary>
        public void ConfigureApp()
        {
            App.PasswordHasher = new PasswordHasher();

            App.Mailer = new Mailer(new NetworkCredential(
                Configuration[Constants.EmailUsername],
                Configuration[Constants.EmailPassword]));

            App.Logger = new Logger(
                Constants.AuthAPI,
                Constants.LogPath,
                60);

            App.CodeGenerator = new CodeGenerator();

            App.ConnectionString = Configuration[Constants.ConnectionString];
        }
    }
}