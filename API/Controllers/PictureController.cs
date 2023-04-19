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
	public class PictureController : BaseController<Picture>
	{
        UnitOfWork uow = new UnitOfWork();
        PictureService pictureService = null;
        public PictureController(GlobalService service, IHttpContextAccessor accessor) : base(service.PictureService, accessor)
        {
            pictureService = service.PictureService;
        }

        [HttpGet("GetPicture")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<PictureResponse>> GetPicture([FromBody] string id)
        {
            ItemResponseModel<PictureResponse> response = new ItemResponseModel<PictureResponse>();

            if (!String.IsNullOrEmpty(id))
            {
                response = await pictureService.Get(id);
                response.HasError = false;
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("ID is empty, please provide an ID");
            }

            return response;
        }

        [HttpGet("GetForAquarium")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<List<PictureResponse>>> GetForAquarium([FromBody]string aquarium)
        {
            ItemResponseModel<List<PictureResponse>> returnModel = new ItemResponseModel<List<PictureResponse>>();

            if (!String.IsNullOrEmpty(aquarium))
            {
                returnModel = await pictureService.GetForAquarium(aquarium);
            }
            else
            {
                returnModel.HasError = true;
                returnModel.ErrorMessages.Add("No Aquarium Name provided");
            }
            return returnModel;
        }

        [HttpPost("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<PictureResponse>> Create(string aquarium, [FromBody] PictureRequest pictureRequest)
        {
            ItemResponseModel<PictureResponse> response = new ItemResponseModel<PictureResponse>();
            if (aquarium != null)
            {
                response = await pictureService.AddPicture(aquarium, pictureRequest);

            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("Aquarium was emtpy");

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

            if(id != null)
            {
                response = await pictureService.Delete(id);
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
}

