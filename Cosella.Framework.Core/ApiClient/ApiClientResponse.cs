﻿using System;

namespace Cosella.Framework.Core.ApiClient
{
    public enum ApiClientResponseStatus
    {
        Ok,
        Exception
    }

    public class ApiClientResponse<T>
    {
        public Exception Exception { get; set; }
        public T Payload { get; set; }
        public ApiClientResponseStatus Status { get; set; }
    }
}