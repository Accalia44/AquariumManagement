using System;
using DAL;
using DAL.Entities;
using DAL.Repository;
using Services.Models.Response;
using SharpCompress.Common;

namespace Services.ImplementedServices
{
    public class CoralService : AquariumItemService
    {
        public CoralService(UnitOfWork uow, IRepository<AquariumItem> repository, GlobalService service) : base(uow, repository, service) { }

        public async Task<ItemResponseModel<Coral>> AddCoral(Coral entity)
        {
            Console.WriteLine(entity.Name);
            ItemResponseModel<Coral> response = new ItemResponseModel<Coral>();
            ItemResponseModel<AquariumItem> resp = await base.AddAquariumItem(entity);
            //Console.WriteLine(resp.Data.Name);

            var aquariumExist = await unitOfWork.Aquarium.FindOneAsync(x => x.Name == entity.Aquarium);

            if (aquariumExist != null)
            {
                if (!String.IsNullOrEmpty(entity.CoralType.ToString()))
                {
                    if (resp.HasError == false)
                    {
                        
                        response.Data = resp.Data as Coral;
                        response.HasError = false;
                    }

                }
                else
                {
                    response.HasError = true;
                    modelStateWrapper.AddError("No CoralType", "Please provide a Coral Type");

                }
            }
            else
            {
                response.HasError = true;
                response.ErrorMessages.Add("No Aquarium was found with that name");
            }
            return response;

        }
        public async Task<ItemResponseModel<List<Coral>>> GetCoral(Aquarium entity)
        {
            var response = new ItemResponseModel<List<Coral>>();

            if (entity != null)
            {
                var coralList = await Get();

                response.Data = coralList.Where(x => x.Aquarium == entity.Name).Where(a => a.GetType() == typeof(Coral)).ToList().Cast<Coral>().ToList();
                response.HasError = false;
            }
            else
            {
                response.HasError = true;
                modelStateWrapper.AddError("No Aquarium", "Please provide an aquarium!");
            }
            return response;
        }

        public override async Task<bool> Validate(AquariumItem entry)
        {
            if (entry.GetType() == typeof(Coral))
            {
                return await base.Validate(entry);
            }
            else
            {
                modelStateWrapper.AddError("NotValid", "Item is no Coral");
            }

            return modelStateWrapper.IsValid;
        }

    }
}

