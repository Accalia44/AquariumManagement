using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.ImplementedServices;
using Services.Models.Request;
using Services.Models.Response;

namespace API.Controllers
{
    public class AquariumController : BaseController<Aquarium>
    {
        UnitOfWork uow = new UnitOfWork();
        AquariumService aquariumService = null;
        public AquariumController(GlobalService service, IHttpContextAccessor accessor) : base(service.AquariumService, accessor)
        {
            aquariumService = service.AquariumService;
        }

        [HttpGet("GetAquarium")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<Aquarium>> GetAquarium([FromBody] string id)
        {

            ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();

            if (!String.IsNullOrEmpty(id))
            {
                response.Data = await aquariumService.Get(id);
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("ID is empty, please provide an ID");
            }

            return response;
        }

        [HttpPost("CreateAquarium")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<Aquarium>> CreateAquarium([FromBody] Aquarium aquarium)
        {

            ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();
            if (aquarium != null)
            {
                response = await aquariumService.Create(aquarium);

            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("Aquarium was emtpy");
            }
            return response;
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<Aquarium>> Edit(string id, [FromBody] Aquarium aquarium)
        {

            ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();

            if (!String.IsNullOrEmpty(id))
            {
                if (!String.IsNullOrEmpty(aquarium.Name))
                {
                    response = await aquariumService.Update(id, aquarium);
                }
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("ID is emtpy");
            }
            return response;
        }

        [HttpGet("ForUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<List<Aquarium>>> ForUser ([FromBody]string username)
        {
            ItemResponseModel<List<Aquarium>> response = new ItemResponseModel<List<Aquarium>>();

            if (!String.IsNullOrEmpty(username))
            {
                User user = uow.User.FindOneAsync(x => username == x.Email).Result;
                response = await aquariumService.GetForUser(user);
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("User Name was empty, please ensure your user is registered before");
            }
            return response;
        }

        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResponseModel> Delete(string id)
        {
            ActionResponseModel response = new ActionResponseModel();

            if (id != null)
            {
                response = await aquariumService.Delete(id);
                response.Success = true;
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("ID was empty");
            }

            return response;
        }

    }

    /*
     * Get ({id})
Create
Edit  ({id})
Coral ({id}/Coral) - Create - Get - GetAll - Edit
Animal ({id}/Animal) - Create - Get - GetAll -  Edit
ForUser (Returns all Aquariums for a specific user)
Picture - Create / Get / Delete
     */

}

