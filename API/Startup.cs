using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Data.Interface;
using API.Data.Repository;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace DatingApp.API {
    public class Startup {
        public Startup (Microsoft.Extensions.Configuration.IConfiguration configuration) {
            Configuration = configuration;
        }

        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            IdentityBuilder builder = services.AddIdentityCore<User> (opt => {
                //opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase=false;
            });

            builder = new IdentityBuilder (builder.UserType, typeof (Role), builder.Services);
            builder.AddEntityFrameworkStores<DataContext> ();
            builder.AddRoleValidator<RoleValidator<Role>> ();
            builder.AddRoleManager<RoleManager<Role>> ();
            builder.AddSignInManager<SignInManager<User>> ();

            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme).AddJwtBearer (options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey (Encoding.ASCII.GetBytes (Configuration.GetSection ("AppSettings:Token").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
                };
            });

            var con = this.Configuration.GetConnectionString ("DefaultConnection");

            services.AddDbContext<DataContext> (x => x.UseSqlServer (con));
            services.AddMvc (options => {
                    //    var policy = new AuthorizationPolicyBuilder ().RequireAuthenticatedUser ().Build ();
                    //   options.Filters.Add (new AuthorizeFilter ());
                }).SetCompatibilityVersion (CompatibilityVersion.Version_2_1)
                .AddJsonOptions (opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            services.Configure<FormOptions> (x => {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = Int64.MaxValue; // In case of multipart
            });
            services.AddCors ();
            services.AddTransient<Seed> ();
            services.AddScoped<IUnitOfWork, UnitOfWork> ();
            services.AddAutoMapper ();
            services.AddSwaggerGen (opt => opt.SwaggerDoc ("v1", new Info { Title = "E Finance Project", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env, Seed seeder) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                //    app.UseHsts();
            }

            //     app.UseHttpsRedirection();

            seeder.SeedUsers ();
            seeder.SeedDepartment ();
            app.UseSwagger ();
            app.UseSwaggerUI (opt => opt
                .SwaggerEndpoint ("/swagger/v1/swagger.json", "E Finance Project v1"));
            app.UseCors (x =>
                x.AllowAnyOrigin ()
                .AllowAnyMethod ()
                .AllowAnyHeader ());
            app.UseAuthentication ();
            app.UseMvc ();

        }
    }
}