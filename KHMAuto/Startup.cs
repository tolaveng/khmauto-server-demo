using System;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Data.Api.Services;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.Mapper;
using Data.Services;
using KHMAuto.Configures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
            services.AddControllers();

            services.AddDbContextPool<AppDataContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("PostgresDatabase")));

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

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
            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                // easy password
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;

            });
            builder = new IdentityBuilder(builder.UserType, typeof(UserRole), builder.Services);
            builder.AddSignInManager<SignInManager<User>>();
            builder.AddRoleManager<RoleManager<UserRole>>();
            builder.AddRoleValidator<RoleValidator<UserRole>>();
            builder.AddEntityFrameworkStores<AppDataContext>().AddDefaultTokenProviders();


            // JWT
            var jwtTokenConfig = Configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();
            services.AddSingleton(jwtTokenConfig);

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false;
                    opt.SaveToken = true; // save to HttpContext
                    opt.TokenValidationParameters = new TokenValidationParameters {
                        NameClaimType = ClaimTypes.NameIdentifier,
                        ValidateIssuer = true,
                        ValidIssuer = jwtTokenConfig.Issuer,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                        ValidateAudience = true,
                        ValidAudience = jwtTokenConfig.Audience,
                        ValidateLifetime = true
                };
            });

            // Add Repositories
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceIndexRepository, ServiceIndexRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Add Services
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IServiceIndexService, ServiceIndexService>();
            services.AddScoped<IUserService, UserService>();
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
