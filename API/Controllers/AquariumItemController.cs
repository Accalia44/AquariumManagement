using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Services;
using Services.ImplementedServices;
using Services.Models.Response;

namespace API.Controllers
{
    public class AquariumItemController : BaseController<AquariumItem>
    {
        UnitOfWork uow = new UnitOfWork();
        AquariumItemService aquariumItemService = null;
        CoralService coralService = null;
        AnimalService animalService = null;

        public AquariumItemController(GlobalService service, IHttpContextAccessor accessor) : base(service.AquariumItemService, accessor) { }
                 
        [HttpPost]
        public async Task<ItemResponseModel<Coral>> Create([FromBody] Coral coral)
        {
            coralService = new CoralService(uow, uow.AquariumItem, null);
            aquariumItemService = new AquariumItemService(uow, uow.AquariumItem, null);
            
            ItemResponseModel<Coral> response = new ItemResponseModel<Coral>();
            if (coral != null)
            {
                return await coralService.AddCoral(coral);

            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("Coral was emtpy");
                return response;
            }
        }

        [HttpPost]
        public async Task<ItemResponseModel<Animal>> Create([FromBody] Animal animal)
        {
            animalService = new AnimalService(uow, uow.AquariumItem, null);
            ItemResponseModel<Animal> response = new ItemResponseModel<Animal>();
            if (animal != null)
            {
                return await animalService.AddAnimal(animal);

            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("Animal was emtpy");
                return response;
            }
        }
        
        [HttpGet]
        public async Task<ItemResponseModel<AquariumItem>> Get([FromBody] string id)
        {
            aquariumItemService = new AquariumItemService(uow, uow.AquariumItem, null);
            ItemResponseModel<AquariumItem> response = new ItemResponseModel<AquariumItem>();

            if (!String.IsNullOrEmpty(id))
            {
                response.Data = await aquariumItemService.Get(id);
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
        public async Task<ItemResponseModel<List<Coral>>> GetAllCorals([FromBody] Aquarium aquarium)
        {
            coralService = new CoralService(uow, uow.AquariumItem, null);

            ItemResponseModel<List<Coral>> response = new ItemResponseModel<List<Coral>>();

            if (!String.IsNullOrEmpty(aquarium.ID))
            {
                response = await coralService.GetCoral(aquarium);
                response.HasError = false;
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("Aquarium ID is empty, please provide a valid Aquarium");
            }

            return response;

        }
        [HttpGet]
        public async Task<ItemResponseModel<List<Animal>>> GetAllAnimals([FromBody] Aquarium aquarium)
        {
            animalService = new AnimalService(uow, uow.AquariumItem, null);
            ItemResponseModel<List<Animal>> response = new ItemResponseModel<List<Animal>>();

            if (!String.IsNullOrEmpty(aquarium.ID))
            {
                response = await animalService.GetAnimal(aquarium);
                response.HasError = false;
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("Aquarium ID is empty, please provide a valid Aquarium");
            }

            return response;

        }

        [HttpPut]
        public async Task<ItemResponseModel<AquariumItem>> Edit(string id, [FromBody] AquariumItem aquariumItem)
        {
            aquariumItemService = new AquariumItemService(uow, uow.AquariumItem, null);
            ItemResponseModel<AquariumItem> response = new ItemResponseModel<AquariumItem>();

            if (!String.IsNullOrEmpty(id))
            {
                if (!String.IsNullOrEmpty(aquariumItem.Name))
                {
                    response = await aquariumItemService.Update(id, aquariumItem);
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
 
    }
}



