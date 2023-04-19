using System;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
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

        public AquariumItemController(GlobalService service, IHttpContextAccessor accessor) : base(service.AquariumItemService, accessor)
        {
            aquariumItemService = service.AquariumItemService;
            coralService = service.CoralService;
            animalService = service.AnimalService;
        }
                 
        [HttpPost("CreateCoral")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<Coral>> CreateCoral ([FromBody] Coral coral)
        {
            
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

        [HttpPost("CreateAnimal")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<Animal>> CreateAnimal ([FromBody] Animal animal)
        {
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
        /*
        [HttpGet("GetAquariumItem")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<AquariumItem>> GetAquariumItem([FromBody] string id)
        {
            ItemResponseModel<AquariumItem> response = new ItemResponseModel<AquariumItem>();

            if (!String.IsNullOrEmpty(id))
            {
                response.Data = await aquariumItemService.Get(id);
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("ID is empty, please provide an ID");
            }

            return response;

        }*/

        [HttpGet("GetAllCorals")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<List<Coral>>> GetAllCorals([FromBody] string aquariumId)
        {
            ItemResponseModel<List<Coral>> response = new ItemResponseModel<List<Coral>>();

            Aquarium found = await uow.Aquarium.FindByIdAsync(aquariumId);

            if (!String.IsNullOrEmpty(found.ID))
            {
                response = await coralService.GetCoral(found);
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("Aquarium ID is empty, please provide a valid Aquarium");
            }

            return response;

        }
        [HttpGet("GetAllAnimals")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<List<Animal>>> GetAllAnimals([FromBody] string aquariumId)
        {
            ItemResponseModel<List<Animal>> response = new ItemResponseModel<List<Animal>>();

            Aquarium found = await uow.Aquarium.FindByIdAsync(aquariumId);

            if (!String.IsNullOrEmpty(found.ID))
            {
                response = await animalService.GetAnimal(found);
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("Aquarium ID is empty, please provide a valid Aquarium");
            }

            return response;

        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ItemResponseModel<AquariumItem>> Edit(string id, [FromBody] AquariumItem aquariumItem)
        {
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
                response = await aquariumItemService.Delete(id);
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



