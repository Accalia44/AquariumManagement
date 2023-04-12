using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
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
        }

        [HttpGet]
        public async Task<ItemResponseModel<Aquarium>> Get([FromBody] string id)
        {
            ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();
            response.Data = await aquariumService.Get(id);
            response.HasError = false;
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<ItemResponseModel<Aquarium>>> Create([FromBody] Aquarium aquarium)
        {
            aquariumService = new AquariumService(uow, uow.Aquarium, null);
            ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();
            if (aquarium != null)
            {
                return await aquariumService.Create(aquarium);

            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("Aquarium was emtpy");
                return response;
            }


        }
        /*
        [HttpPut]
        public async Task<ItemResponseModel<Aquarium>> Edit(string id, [FromBody] Aquarium aquarium)
        {

        }*/
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

