using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.DependencyInjection;
using Server.Api.Repositories;
using Microsoft.Extensions.Configuration;

namespace Server.Api.Services
{
    public class HostedServices : IHostedService, IDisposable
    {       
        private readonly IServiceScopeFactory _scopeFactory;

        public HostedServices(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var questionRepository = scope.ServiceProvider.GetRequiredService<IQuestionRepository>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                Receiver r = new(configuration, questionRepository);
                return r.Run();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
        
        public void Dispose()
        {
            // return Task.FromResult(0);
        }
    }
}