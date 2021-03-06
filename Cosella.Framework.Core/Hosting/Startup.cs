﻿using Cosella.Framework.Core.Integrations.Swagger;
using Cosella.Framework.Core.VersionTracking;
using log4net;
using Microsoft.Owin.Cors;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;
using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Cosella.Framework.Core.Hosting
{
    public class Startup
    {
        private IKernel _kernel;

        public Startup(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Configuration(IAppBuilder app)
        {
            var serviceConfiguration = _kernel.Get<HostedServiceConfiguration>();
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes(new RoutePrefixProvider(serviceConfiguration));
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(config.Formatters.JsonFormatter);

            config
                .EnableSwagger(swaggerConfig =>
                {
                    swaggerConfig.SingleApiVersion(
                        $"v{serviceConfiguration.RestApiVersion}",
                        $"{serviceConfiguration.ServiceName} API");

                    swaggerConfig.OperationFilter<RolesOperationFilter>();
                })
                .EnableSwaggerUi();

            app
                .UseCors(CorsOptions.AllowAll)
                .UseVerionTracking(_kernel)
                .UseNinjectMiddleware(() => _kernel)
                .UseNinjectWebApi(config);
        }
    }
}