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
                response.Data = await repository.UpdateOneAsync(entity); ;
                response.HasError = false;
                return response;
            }

            else
            {
                modelStateWrapper.AddError("AquariumItem Not Found", "AquariumItem was not in Database");
                return response;
            }
        }

        public override async Task<AquariumItem> Get(string id)
        {
            AquariumItem found = await this.repository.FindByIdAsync(id);
            if (!String.IsNullOrEmpty(found.ID))
            {

            }
            else
            {
                modelStateWrapper.AddError("No AquariumItem found", "Please provide an existing AquariumItem");
            }
            return found;

        }

        public async Task<ItemResponseModel<AquariumItem>> AddAquariumItem(AquariumItem entity)
        {

            ItemResponseModel<AquariumItem> response = new ItemResponseModel<AquariumItem>();

            var foundAquarium = await unitOfWork.Aquarium.FindOneAsync(x => x.Name == entity.Aquarium);

            if (foundAquarium != null)
            {
                response = await CreateHandler(entity); 
                response.HasError = false;
            }
            else
            {
                modelStateWrapper.AddError("No Aquarium", "This Aquarium does not exist");
                response.HasError = true;
            }

            return response;
        }

        public override async Task<bool> Validate(AquariumItem entry)
        {
            if (entry != null)
            {
                if (entry.Inserted <= DateTime.MinValue)
                {
                    modelStateWrapper.AddError("InsertedMissing", "No insert date was set");
                }
                if (entry.Amount <= 0)
                {
                    modelStateWrapper.AddError("AmountMissing", "Amount must be greater 0");
                }
            }
            else
            {
                modelStateWrapper.AddError("ItemEmpty", "Object is empty");
            }
            Console.WriteLine("test");
            Console.WriteLine(modelStateWrapper.IsValid);
            return modelStateWrapper.IsValid;

        }

    }
}
