using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChatSample.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Api.DataBase;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Services;

namespace Server.Api
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

            // Setup cors
            services.AddCors(options =>
            {
                options.AddPolicy("PolicyName", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            // Setup database
            services.AddDbContext<DataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<DataContext>();

            // Setup Identity Service
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            });

            // Auto Mapper Configurations
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Authentication JWT SetUp
            var jwtSettings = Configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.GetSection("secretKey").Value))
                };

                // event for signalR
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Path.ToString().StartsWith("/chat"))
                            context.Token = context.Request.Query["access_token"];
                        return Task.CompletedTask;
                    },
                };
            });

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

            // Anti JSON looping
            services.AddControllers().AddNewtonsoftJson(options =>
              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // Custom
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<ITokenManager, JwtTokenManager>();
            services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());
            // services.AddScoped<IUserRepository, PgUserRepository>(); VERVANGEN DOOR GEBRUIK VAN ASP.NET UserManager
            services.AddScoped<ITopicRepository, PgTopicRepository>();
            services.AddScoped<IAnswerRepository, PgAnswerRepository>();
            services.AddScoped<IQuestionRepository, PgQuestionRepository>();
            services.AddScoped<IFieldOfStudyRepository, PgFieldOfStudyRepository>();
            services.AddScoped<ICourseRepository, PgCourseRepository>();
            services.AddScoped<IChannelRepository, pgChannelRepository>();
            services.AddScoped<pgMessageRepository, pgMessageRepository>();
            services.AddScoped<ICourseSubscriptionRepository, PgCourseSubscriptionRepository>();
            services.AddScoped<IQuestionSubscriptionRepository, PgQuestionSubscriptionRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server.Api", Version = "v1" });
            });

            //signalR
            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            // Email FluentMail
            setupFluentEmail(services);

            // Email MailKit
            //services.AddSingleton<Receiver>();
            //Receiver receiver = new(Configuration); // TODO inject??
            //services.AddSingleton<IHostedService, HostedServices>();

        }

        private void setupFluentEmail(IServiceCollection services)
        {
            string from = Configuration.GetSection("Mail")["From"];
            string senderEmail = Configuration.GetSection("Mail")["G-SenderEmail"];
            string password = Configuration.GetSection("Mail")["G-password"];
            string G_host = Configuration.GetSection("Mail")["G-host"];
            int G_port = Convert.ToInt32(Configuration.GetSection("Mail")["G-port"]);

            SmtpClient smtp = new SmtpClient
            {
                Host = G_host,
                Port = G_port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, password)
            };

            services
                .AddFluentEmail(senderEmail, from)
                .AddRazorRenderer()
                .AddSmtpSender(smtp);

            services.TryAddScoped<IEmailSender, Mailer>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server.Api v1"));
            }

            // app.UseHttpsRedirection();

            //ENABLE CORS
            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true) // allow any origin
               .AllowCredentials()); // allow credentials

            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Required for Authentication!!
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");

            });

        }
    }
}
