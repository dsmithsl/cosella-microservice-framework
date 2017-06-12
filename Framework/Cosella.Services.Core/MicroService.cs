﻿using Cosella.Services.Core.Hosting;
using System;
using System.Reflection;
using Topshelf;
using Topshelf.ServiceConfigurators;

namespace Cosella.Services.Core
{
    public class MicroService
    {
        private HostedServiceConfiguration _configuration;
        private HostedServiceConfiguration _defaultConfiguration;

        public HostedServiceConfiguration Configuration { get { return _configuration; } }

        public MicroService(HostedServiceConfiguration configuration)
        {
            _configuration = configuration;

            string serviceName = Assembly.GetEntryAssembly().GetName().Name.Replace(" ", "");
            _defaultConfiguration = new HostedServiceConfiguration()
            {
                ServiceName = serviceName,
                ServiceDisplayName = serviceName,
                ServiceDescription = "",
                ServiceInstanceName = serviceName
            };
        }

        public static MicroService Create(Action<HostedServiceConfiguration> configurator)
        {
            log4net.Config.XmlConfigurator.Configure();

            var configuration = new HostedServiceConfiguration();
            configurator(configuration);
            return new MicroService(configuration);
        }

        public int Run()
        {
            int exitCode = (int)HostFactory.Run(config =>
            {
                config.UseLog4Net();

                config.SetServiceName(_configuration.ServiceName ?? _defaultConfiguration.ServiceName);
                config.SetDisplayName(_configuration.ServiceDisplayName ?? _defaultConfiguration.ServiceDisplayName);
                config.SetInstanceName(_configuration.ServiceInstanceName ?? _defaultConfiguration.ServiceInstanceName);
                config.SetDescription(_configuration.ServiceDescription ?? _defaultConfiguration.ServiceDescription);

                config.Service<HostedService>(service =>
                {
                    ServiceConfigurator<HostedService> hostedService = service;
                    hostedService.ConstructUsing(() => HostedService.Create(_configuration));
                    hostedService.WhenStarted((s, hostControl) => s.Start(hostControl));
                    hostedService.WhenStopped(s => s.Stopped());
                    hostedService.WhenPaused(s => s.Paused());
                    hostedService.WhenContinued(s => s.Continued());
                    hostedService.WhenShutdown(s => s.Shutdown());
                });
            });

#if DEBUG
            // Wait for a key if debugging
            Console.ReadKey();
#endif
            return exitCode;
        }
    }
}