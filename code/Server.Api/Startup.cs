using System;
using System.Net;
using AutoMapper;
using System.Text;
using System.Net.Mail;
using ChatSample.Hubs;
using Server.Api.Models;
using Server.Api.Services;
using Server.Api.DataBase;
using System.Threading.Tasks;
using Server.Api.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Server.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options => {
                options.AddPolicy("PolicyName", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddDbContext<DataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<DataContext>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options => {
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var jwtSettings = Configuration.GetSection("JwtSettings");
            services.AddAuthentication(opt => {
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("secretKey").Value))
                };

                options.Events = new JwtBearerEvents {
                    OnMessageReceived = context => {
                        if (context.Request.Path.ToString().StartsWith("/chat")) {
                            context.Token = context.Request.Query["access_token"];
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddSession();

            services.AddControllers().AddNewtonsoftJson(options => {
              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<ITokenManager, JwtTokenManager>();
            services.AddScoped<ITopicRepository, PgTopicRepository>();
            services.AddScoped<IAnswerRepository, PgAnswerRepository>();
            services.AddScoped<ICourseRepository, PgCourseRepository>();
            services.AddScoped<IChannelRepository, PgChannelRepository>();
            services.AddScoped<IMessageRepository, PgMessageRepository>();
            services.AddScoped<IQuestionRepository, PgQuestionRepository>();
            services.AddScoped<IFieldOfStudyRepository, PgFieldOfStudyRepository>();
            services.AddScoped<IDataContext>(provider => provider.GetService<DataContext>());
            services.AddScoped<ISubscriptionRepository<CourseSubscription>, PgCourseSubscriptionRepository>();
            services.AddScoped<INotificationRepository<AnswerNotification>, PgAnswerNotificationRepository>();
            services.AddScoped<ISubscriptionRepository<QuestionSubscription>, PgQuestionSubscriptionRepository>();
            services.AddScoped<INotificationRepository<QuestionNotification>, PgQuestionNotificationRepository>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server.Api", Version = "v1" });
            });

            services.AddSignalR();
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            setupEmail(services);
        }

        private void setupEmail(IServiceCollection services)
        {
            string from = Configuration.GetSection("Mail")["From"];
            string G_host = Configuration.GetSection("Mail")["host"];
            string password = Configuration.GetSection("Mail")["password"];
            string senderEmail = Configuration.GetSection("Mail")["SenderEmail"];
            int G_port = Convert.ToInt32(Configuration.GetSection("Mail")["port"]);

            SmtpClient smtp = new SmtpClient {
                Host = G_host,
                Port = G_port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(senderEmail, password)
            };

            services
                .AddFluentEmail(senderEmail, from)
                .AddRazorRenderer()
                .AddSmtpSender(smtp);

            services.TryAddScoped<IEmailSender, Mailer>();

            services.AddHostedService<MailListener>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server.Api v1"));
            }

            app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .SetIsOriginAllowed(origin => true)
               .AllowCredentials());

            app.UseRouting();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}
