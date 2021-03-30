using System;
using System.Text;
using AutoMapper;
using Data.Api.Services;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.Interfaces;
using Data.Mapper;
using Data.Services;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace KHMAuto
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
            // JSON Format
            services.AddControllers(opt =>
            {
                // All end points are required authentication
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            })
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            //services.AddControllers()
            //.AddJsonOptions(options =>
            //   options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });



            // Identity
            IdentityBuilder identityBuilder = services.AddIdentityCore<User>(opt =>
            {
                // easy password
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

            });
            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(UserRole), identityBuilder.Services);
            identityBuilder.AddSignInManager<SignInManager<User>>();
            identityBuilder.AddRoleManager<RoleManager<UserRole>>();
            identityBuilder.AddRoleValidator<RoleValidator<UserRole>>();
            identityBuilder.AddEntityFrameworkStores<AppDataContext>().AddDefaultTokenProviders();


            // JWT
            var jwtConfig = Configuration.GetSection("JwtTokenConfig").Get<JwtConfig>();
            services.AddSingleton(jwtConfig);

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true; // save to HttpContext
                    opt.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),

                        //NameClaimType = ClaimTypes.NameIdentifier,
                        ValidateIssuer = false,
                        //ValidIssuer = jwtTokenConfig.Issuer,
                        ValidateAudience = false,
                        //ValidAudience = jwtTokenConfig.Audience,
                        ValidateLifetime = true
                };
            });


            // Data
            services.AddDbContextPool<AppDataContext>(options =>
            //    options.UseNpgsql(Configuration.GetConnectionString("PostgresDd"))
                options.UseMySql(Configuration.GetConnectionString("MariaDb"))
            );
            services.AddAutoMapper(typeof(AutoMapperProfile));


            // Add Repositories
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceIndexRepository, ServiceIndexRepository>();

            // Add Services
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IServiceIndexService, ServiceIndexService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
