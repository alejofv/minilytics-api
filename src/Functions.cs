using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Alejof.Minilytics
{
    public class Functions
    {
        public const string QueueName = "minilytics-log";
        public const string TableName = "MinilyticsEntries";
        public const string StorageConnectionString = "StorageConnectionString";

        [FunctionName(nameof(Warmup))]
        public IActionResult Warmup(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "warmup")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# Http trigger function processed: {nameof(Warmup)}");
            return new OkResult();
        }

        [FunctionName(nameof(Log))]
        public async Task<IActionResult> Log(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "log/{source}")] HttpRequest req, string source,
           [Queue(QueueName, Connection = StorageConnectionString)] IAsyncCollector<Models.HttpHit> collector,
            ILogger log)
        {
            log.LogInformation($"C# Http trigger function processed: {nameof(Log)}");

            var content = await req.ReadAsStringAsync();
            var visit = JsonConvert.DeserializeObject<Models.HttpHit>(content);
            visit.Source = source;

            await collector.AddAsync(visit);
            return new OkResult();
        }

        [FunctionName(nameof(Save))]
        public async Task Save(
           [QueueTrigger(QueueName, Connection = StorageConnectionString)] Models.HttpHit item,
           [Table(TableName, Connection = StorageConnectionString)] IAsyncCollector<Models.LogEntry> collector,
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {nameof(Save)}");
            await collector.AddAsync(new Models.LogEntry(item.Source, item.Url) { Method = item.Method });
        }
    }
}
