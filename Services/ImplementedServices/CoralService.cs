using System;
using DAL;
using DAL.Entities;
using DAL.Repository;
using Services.Models.Response;

namespace Services.ImplementedServices
{
    public class CoralService : AquariumItemService
    {
        public CoralService(UnitOfWork uow, IRepository<AquariumItem> repository, GlobalService service) : base(uow, repository, service) { }

        public async Task<ItemResponseModel<Coral>> AddCoral(Coral entity)
        {
            ItemResponseModel<Coral> response = new ItemResponseModel<Coral>();

            if (await Validate(entity))
            {
                if (String.IsNullOrEmpty(entity.CoralType.ToString()))
                {
                    response.HasError = true;
                    modelStateWrapper.AddError("No Coral", "No Coral Type was provided");
                    return response;

                }
                else
                {
                    await this.AddAquariumItem(entity);
                    response.Data = entity;
                    response.HasError = false;
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

        public async Task<ItemResponseModel<List<AquariumItem>>> GetCoral(Aquarium entity)
        {
            var response = new ItemResponseModel<List<AquariumItem>>();

            if (entity != null)
            {
                var coralList = await Get();

                response.Data = coralList.Where(x => x.Aquarium == entity.Name).Where(a => a.GetType() == typeof(Coral)).ToList();
                response.HasError = false;
            }
            else
            {
                response.HasError = true;
                modelStateWrapper.AddError("No Aquarium", "Please provide an aquarium!");
            }
            return response;
        }



        public async Task<bool> Validate(Coral entity)
        {
            if (entity != null)
            {
                var aquariumExist = await unitOfWork.Aquarium.FindOneAsync(x => x.Name == entity.Aquarium);

                if (aquariumExist != null)
                {

                }
                else
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

