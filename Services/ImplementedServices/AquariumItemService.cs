using System;
using DAL;
using DAL.Entities;
using DAL.Repository;
using Services.Models.Response;

namespace Services.ImplementedServices
{
    public class AquariumItemService : Service<AquariumItem>
    {
        public AquariumItemService(UnitOfWork uow, IRepository<AquariumItem> repository, GlobalService service) : base(uow, repository, service){}

        public async override Task<ItemResponseModel<AquariumItem>> Create(AquariumItem entity)
        {
            ItemResponseModel<AquariumItem> response = new ItemResponseModel<AquariumItem>();

            AquariumItem newAquariumItem = await this.unitOfWork.AquariumItem.InsertOneAsync(entity);

            response.Data = newAquariumItem;
            response.HasError = false;
            return response;
        }

        public async override Task<ItemResponseModel<AquariumItem>> Update(string id, AquariumItem entity)
        {
            ItemResponseModel<AquariumItem> response = new ItemResponseModel<AquariumItem>();

            var foundAquariumItem = await this.repository.FindByIdAsync(entity.ID);
            if (foundAquariumItem != null)
            {
                response.Data = entity;
                response.HasError = false;
                return response;
            }

            else
            {
                modelStateWrapper.AddError("AquariumItem Not Found", "AquariumItem was not in Database");
                return response;
            }
        }
    

        public async override Task<bool> Validate(AquariumItem entity)
        {
            if (entity != null)
            {
                if (String.IsNullOrEmpty(entity.Aquarium))
                {
                    modelStateWrapper.AddError("No Aquarium", "Please provide an Aquarium");
                }

                if (String.IsNullOrEmpty(entity.Name))
                {
                    modelStateWrapper.AddError("No Name", "Please provide a Name for your Animal");
                }
                if (String.IsNullOrEmpty(entity.Species))
                {
                    modelStateWrapper.AddError("No Species", "Please provide a Species");
                }
            }
            else
            {
                modelStateWrapper.AddError("No AquariumItem", "No AquariumItem was provided");

            }
            return modelStateWrapper.IsValid;
        }

        public async Task<ItemResponseModel<AquariumItem>> AddAquariumItem(AquariumItem entity)
        {
            ItemResponseModel<AquariumItem> response = new ItemResponseModel<AquariumItem>();

            if (await Validate(entity))
            {
                var aquarium = entity.Aquarium;
                var foundAquarium = await unitOfWork.Aquarium.FindOneAsync(x => x.Name == aquarium);

                if (foundAquarium != null)
                {
                    AquariumItem newAquariumItem = await this.unitOfWork.AquariumItem.InsertOneAsync(entity);
                    response.Data = newAquariumItem;
                    response.HasError = false;
                    return response;
                }
                else
                {
                    modelStateWrapper.AddError("No Aquarium", "This Aquarium does not exist");
                    response.HasError = true;
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


    }
}
