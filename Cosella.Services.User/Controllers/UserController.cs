﻿namespace Cosella.Services.User.Controllers
{
    using Framework.Contracts;
    using Framework.Core.Attributes;
    using Framework.Core.Configuration;
    using Framework.Core.Controllers;
    using log4net;
    using System.Net;
    using System.Web.Http;

    [RoutePrefix("users")]
    public class UserController : RestApiController
    {
        private ILog _log;
        private IConfigurator _configurator;

        public UserController(ILog log, IConfigurator configurator)
        {
            _log = log;
            _configurator = configurator;
        }

        [Route("")]
        [HttpGet]
        [Roles(new[] { "super:user:read" })]
        public IHttpActionResult GetAll()
        {
            return Ok();
        }

        [Route("authenticate")]
        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] AuthenticationRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return Content(HttpStatusCode.BadRequest, "Invalid username or password in data body");
            }

            if (request.Username == "test" &&
                request.Password == "test")
            {
                return Ok(new AuthenticationResult()
                {
                    IsAuthorised = true,
                    Name = "Test User",
                    Roles = new string[] { "super:users:read" },
                    Username = request.Username
                });
            }

            return Unauthorized();
        }
    }
}