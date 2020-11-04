using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Alejof.Minilytics.Models
{
    public class HttpHit
    {
        public string Source { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
    }

    public class LogEntry : TableEntity
    {
        public string Method { get; set; }

        public LogEntry(string source, string url)
        {
            var safeUrl = url.Replace("/", "-");
            if (!safeUrl.StartsWith("-"))
                safeUrl = "-" + safeUrl;
            
            this.PartitionKey = $"{source}{safeUrl}";
            this.RowKey = Guid.NewGuid().ToString();
        }
    }
}