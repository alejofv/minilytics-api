using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MediatR;

namespace Alejof.Minilytics
{
    public class Functions
    {
        private readonly IMediator _mediator;

        public Functions(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName(nameof(Warmup))]
        public IActionResult Warmup(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "warmup")]HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# Http trigger function processed: {nameof(Warmup)}");
            return new OkResult();
        }

        [FunctionName(nameof(Log))]
        public IActionResult Log(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "log")]HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# Http trigger function processed: {nameof(Log)}");
            return new OkResult();
        }
    }
}
