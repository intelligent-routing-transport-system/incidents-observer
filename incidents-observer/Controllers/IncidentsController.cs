using incidents_observer.Repository.UnityOfWork;
using incidents_observer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace incidents_observer.Controllers
{
    public class IncidentsController : Controller
    {
        private readonly IUnityOfWork _uow;
        private readonly IConfiguration _configuration;

        public IncidentsController(IUnityOfWork uow, IConfiguration configuration)
        {
            _uow = uow;
            _configuration = configuration;
        }

        public async Task Index()
        {
            while (true)
            {
                var payloadConsumer = new ConsumeMessageFlow(_configuration).Run();
                var messageSend = await new ProduceMessageFlow(_configuration, _uow, payloadConsumer).Run();
                Console.WriteLine(messageSend);
            }
        }
    }
}
