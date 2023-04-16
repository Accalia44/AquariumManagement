using System;
using DAL;
using DAL.Entities;
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
        public PictureController(GlobalService service, IHttpContextAccessor accessor) : base(service.PictureService, accessor) { }

        //Picture - Create / Get / Delete
        [HttpGet]
        public async Task<ItemResponseModel<PictureResponse>> Get([FromBody] string id)
        {
            //helperline
            pictureService = new PictureService(uow, uow.Picture, null);

            ItemResponseModel<PictureResponse> response = new ItemResponseModel<PictureResponse>();

            if (!String.IsNullOrEmpty(id))
            {
                response = await pictureService.GetPicture(id);
                response.HasError = false;
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("ID is empty, please provide an ID");
            }

            return response;
        }

        [HttpGet]
        public async Task<ItemResponseModel<List<PictureResponse>>> GetForAquarium(string aquarium)
        {
            //helperline
            pictureService = new PictureService(uow, uow.Picture, null);

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

        [HttpPost]
        public async Task<ItemResponseModel<PictureResponse>> Create([FromBody] string aquarium, PictureRequest pictureRequest)
        {
            //helperline
            pictureService = new PictureService(uow, uow.Picture, null);

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

        [HttpDelete]
        public async Task<ActionResponseModel> Delete([FromBody] string id)
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

