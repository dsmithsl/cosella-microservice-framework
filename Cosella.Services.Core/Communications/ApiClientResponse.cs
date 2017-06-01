﻿using System;

namespace Cosella.Services.Core.Communications
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