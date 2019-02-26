/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Startup
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *               <DavidPetr>       <david.petrosyan11100@gmail.com>
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
using DbConnectClient;
using MailSevice;
using PasswordHasherService;

namespace AuthAPI
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
            Configuration = configuration;
        }
        
        /// <summary>
        /// Configures services
        /// </summary>
        /// <param name="services">services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            App.DataClient = new DataClient(
                IPAddress.Parse(Configuration[Constants.DbConnectHost]),
                int.Parse(Configuration[Constants.DbConnectPort]));

            App.PasswordHasher = new PasswordHasher();

            App.Mailer = new Mailer(new NetworkCredential(
                Configuration[Constants.EmailUsername],
                Configuration[Constants.EmailPassword]));
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
        }
    }
}