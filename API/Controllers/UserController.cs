using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models.Request;
using Services.Models.Response;

namespace API.Controllers
{
	public class UserController : BaseController<User>
	{
		UserServices userService = null;
        UnitOfWork uow = new UnitOfWork();

        public UserController(GlobalService service, IHttpContextAccessor accessor ) : base(service.UserService, accessor)
		{
			this.userService = service.UserService;
		}

		[HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemResponseModel<UserResponse>>> Login([FromBody]LoginRequest request)
		{
			return await userService.Login(request);
		}
		/*
        public async Task<ItemResponseModel<User>> Update(string id, User entity)
		{


		}

		Delete/Register
		*/
    }
}

