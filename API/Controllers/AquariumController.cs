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
        public AquariumController(GlobalService service, IHttpContextAccessor accessor) : base(service.AquariumService, accessor){}

        [HttpGet]
        public async Task<ItemResponseModel<Aquarium>> Get([FromBody] string id)
        {
            //helperline
            aquariumService = new AquariumService(uow, uow.Aquarium, null);

            ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();

            if (!String.IsNullOrEmpty(id))
            {
                 response.Data = await aquariumService.Get(id);
                response.HasError = false;
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("ID is empty, please provide an ID");
            }

            return response;
        }

        [HttpPost]
        public async Task<ItemResponseModel<Aquarium>> Create([FromBody] Aquarium aquarium)
        {
            //helperline
            aquariumService = new AquariumService(uow, uow.Aquarium, null);

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
        
        [HttpPut]
        public async Task<ItemResponseModel<Aquarium>> Edit(string id, [FromBody] Aquarium aquarium)
        {
            //helperline
            aquariumService = new AquariumService(uow, uow.Aquarium, null);

            ItemResponseModel<Aquarium> response = new ItemResponseModel<Aquarium>();

            if (!String.IsNullOrEmpty(id))
            {
                if (!String.IsNullOrEmpty(aquarium.Name))
                {
                    response = await aquariumService.Update(id, aquarium);
                }
                response.HasError = true;
                response.ErrorMessages.Add("Name is empty");
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("ID is emtpy");
            }
            return response;
        }

        [HttpGet]
        public async Task<ItemResponseModel<List<Aquarium>>> ForUser ([FromBody]User user)
        {
            //helperline
            aquariumService = new AquariumService(uow, uow.Aquarium, null);
            ItemResponseModel<List<Aquarium>> response = new ItemResponseModel<List<Aquarium>>();

            if (!String.IsNullOrEmpty(user.ID))
            {
                response = await aquariumService.GetForUser(user);
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("User ID empty, please ensure your user is registered before");
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

