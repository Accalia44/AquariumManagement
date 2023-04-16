using System;
using DAL;
using DAL.Entities;
using DAL.Repository;
using Microsoft.IdentityModel.Tokens;
using Services.Models.Request;
using Services.Models.Response;

namespace Services.ImplementedServices
{
	public class AnimalService : AquariumItemService
	{
        public AnimalService(UnitOfWork uow, IRepository<AquariumItem> repository, GlobalService service) : base(uow, repository, service){ }

        public async Task<ItemResponseModel<Animal>> AddAnimal (Animal entity)
        {
            ItemResponseModel<Animal> response = new ItemResponseModel<Animal>();

            if (await Validate(entity))
            {
                if (entity.IsAlive.Equals(true))
                {
                    await this.AddAquariumItem(entity);
                    response.Data = entity;
                    response.HasError = false;
                    return response;
                }
                else
                {
                    response.HasError = true;
                    modelStateWrapper.AddError("No Animal", "Animal is already Dead");
                    return response;
                }
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages = modelStateWrapper.Errors.Values.ToList();
                return response;
            }        
        }

        public async Task<ItemResponseModel<List<Animal>>> GetAnimal(Aquarium entity)
        {
            var response = new ItemResponseModel<List<Animal>>();

            if (entity != null)
            {
                var animalList = await Get();

                response.Data = animalList.Where(x => x.Aquarium == entity.Name).Where(a => a.GetType() == typeof(Animal)).ToList().Cast<Animal>().ToList();
                response.HasError = false;
            }
            else
            {
                response.HasError = true;
                modelStateWrapper.AddError("No Aquarium", "Please provide an aquarium!");
            }
            return response;
        }

        public async Task<bool> Validate(Animal entity)
        {
            if (entity != null)
            {
                var aquariumExist = await unitOfWork.Aquarium.FindOneAsync(x => x.Name == entity.Aquarium);

                if(aquariumExist == null)
                {
                    modelStateWrapper.AddError("No Aquarium", "No aquarium was found, please ensure your Aquarium is inserted first");
                }

            }
            else
            {
                modelStateWrapper.AddError("No Animal", "No animal was provided");

            }
            return modelStateWrapper.IsValid;
        }
    }
}

