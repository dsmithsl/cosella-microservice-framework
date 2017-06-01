﻿using Newtonsoft.Json;

namespace Cosella.Services.Core.Models
{
    public class ConsulHealthCheck
    {
        [JsonProperty("DeregisterCriticalServiceAfter")]
        public string DeregisterCriticalServiceAfter { get; set; }

        [JsonProperty("Script")]
        public string Script { get; set; }

        [JsonProperty("HTTP")]
        public string Http { get; set; }

        [JsonProperty("Interval")]
        public string Interval { get; set; }

        [JsonProperty("TTL")]
        public string Ttl { get; set; }
    }
}