using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Api.Services;
using Data.Api.SignalR;
using Data.BackgrooundServices;
using Data.Domain.Models;
using Data.Domain.Repositories;
using Data.Interfaces;
using Data.Mapper;
using Data.Services;
using Infrastructure;
using KHMAuto.Middlewares;
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
using Microsoft.OpenApi.Models;

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

            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(policy =>
            //        policy.SetIsOriginAllowed(_ => true)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials()    // SignalR
            //        .WithExposedHeaders("Content-Disposition")  // download file;
            //        );

            //    //options.AddPolicy("CorsPolicy",
            //    //    policy =>
            //    //    {
            //    //        policy
            //    //        .AllowAnyOrigin()
            //    //        .AllowAnyMethod()
            //    //        .AllowAnyHeader()
            //    //        //.WithOrigins("http://localhost")
            //    //        .AllowCredentials() // required for SignalR
            //    //        .WithExposedHeaders("Content-Disposition"); // download file
            //    //    });
            //});

            services.AddCors(options =>
            {
               options.AddPolicy("CorsPolicy",
                   builder => builder
                   .AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .WithExposedHeaders("Content-Disposition") // download file
                   );

               options.AddPolicy("SignalR",
                   builder => builder
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials()
                   .WithExposedHeaders("Content-Disposition")  // download file
                   .SetIsOriginAllowed(hostName => true));
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
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero // remove 5 minute window after the token expired,
                    };

                    // Add Signal access token
                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/backup"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });


            // Data
            services.AddDbContextPool<AppDataContext>(options => {
                //  options.UseNpgsql(Configuration.GetConnectionString("PostgresDd"));
                options.UseMySQL(Configuration.GetConnectionString("MariaDb"));
                //options.EnableSensitiveDataLogging(true);
                }
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
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // Add Services
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IServiceService, ServiceService>();
            services.AddScoped<IServiceIndexService, ServiceIndexService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();

            services.AddSingleton<IBackupJob, BackupJob>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                     },
                     new string[] { }
                   }
                 });
             });

            // SignalR
            services.AddSignalR();

            // Backup service
            //services.AddHostedService<BackupService>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionMiddleware();

            // app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "KHM Auto");
                c.RoutePrefix = string.Empty; // serve on app root
            });

            app.UseRouting();
            //app.UseCors("CorsPolicy");
            app.UseCors("SignalR");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<BackupHub>("/backuphub");
            });
        }
    }
}
