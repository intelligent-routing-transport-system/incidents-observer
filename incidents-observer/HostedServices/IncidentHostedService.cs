using incidents_observer.Repository.UnityOfWork;
using incidents_observer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace incidents_observer.HostedServices
{
    public class IncidentServiceHosted : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IncidentServiceHosted(ILogger<IncidentServiceHosted> logger, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            new Timer(ExecuteProcess, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            Task.WhenAll(Task.CompletedTask);
            return Task.CompletedTask;
        }

        private async void ExecuteProcess(object state)
        {
            _logger.LogInformation("### Consume Message Proccess executing ###");
            var payloadConsumer = new ConsumeMessageFlow(_configuration).Run();
            _logger.LogInformation("### Produce Message Proccess executing ###");
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _uow = scope.ServiceProvider.GetService<IUnityOfWork>();
                var messageSend = await new ProduceMessageFlow(_configuration, _uow, payloadConsumer).Run();
                _logger.LogInformation($"### {payloadConsumer} ###");
            }
            _logger.LogInformation("### Finish ###");
            _logger.LogInformation($"{DateTime.Now}");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("### Proccess stoping ###");
            _logger.LogInformation($"{DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
