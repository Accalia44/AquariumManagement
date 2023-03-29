using System;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DAL.Entities;
using Services;
using Serilog;
using Utils;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : Entity
    {
        protected Service<T> _Service = null;
        protected Serilog.ILogger log = Logger.ContextLog<BaseController<T>>();
        protected String UserEmail = null;
        protected ClaimsPrincipal ClaimsPrincipal = null;
        public BaseController(Service<T> service, IHttpContextAccessor accessor)
        {
            this._Service = service;
            Task model = this._Service.SetModelState(this.ModelState);
            model.Wait();

            if(accessor != null)
            {
                if(accessor.HttpContext != null)
                {
                    if(accessor.HttpContext.User != null)
                    {
                        ClaimsPrincipal = accessor.HttpContext.User;

                        var identity = (ClaimsIdentity)ClaimsPrincipal.Identity;
                        if(identity != null)
                        {
                            IEnumerable<Claim> claims = identity.Claims;

                            Claim email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

                            if( email != null)
                            {
                                UserEmail = email.Value;
                                Task loadUser = this._Service.Load(UserEmail);
                                loadUser.Wait();
                            }
                            else
                            {
                                log.Debug("Email is null");
                            }
                        }
                        else
                        {
                            log.Debug("Identity is null");
                        }
                    }
                    else
                    {
                        log.Debug("User is null");
                    }
                }
                else
                {
                    log.Debug("HTTP Context is null");
                }

            }
            else
            {
                log.Debug("Accessor is null");
            }
        }

        // HttpGet muss genau gleich heißen wie übergebener Parameter benannt ist 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<T>> Get(string id)
        {
            T result = await _Service.Get(id);
            if(result == null)
            {
                return new NotFoundObjectResult(null);

            }
            return result;
        }
    }

}

